FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SpelMoment.WebAPI/SpelMoment.WebAPI.csproj", "SpelMoment.WebAPI/"]
COPY ["SpelMoment.Domain/SpelMoment.Domain.csproj", "SpelMoment.Domain/"]
COPY ["SpelMoment.Infrastructure/SpelMoment.Infrastructure.csproj", "SpelMoment.Infrastructure/"]

RUN dotnet restore "SpelMoment.WebAPI/SpelMoment.WebAPI.csproj"

COPY . .
RUN dotnet publish "SpelMoment.WebAPI/SpelMoment.WebAPI.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SpelMoment.WebAPI.dll"]