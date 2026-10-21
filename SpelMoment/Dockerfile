FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SpelMoment/SpelMoment.csproj", "SpelMoment/"]
COPY ["SpelMoment.Domain/SpelMoment.Domain.csproj", "SpelMoment.Domain/"]
COPY ["SpelMoment.Infrastructure/SpelMoment.Infrastructure.csproj", "SpelMoment.Infrastructure/"]
RUN dotnet restore "SpelMoment/SpelMoment.csproj"
COPY . .
WORKDIR "/src/SpelMoment"
RUN dotnet build "./SpelMoment.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SpelMoment.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpelMoment.dll"]
