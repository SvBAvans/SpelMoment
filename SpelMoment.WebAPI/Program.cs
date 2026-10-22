using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Infrastructure.Data;
using SpelMoment.Infrastructure.Repositories;
using SpelMoment.WebAPI.GraphQL;
using SpelMoment.WebAPI.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is empty.");

builder.Services.AddDbContext<SpelMomentDbContext>(options =>
{
    options
        .UseSqlServer(dbConnectionString, sqlOptions =>
        {
            sqlOptions.MigrationsHistoryTable("EFMigrationHistory", SpelMomentDbContext.SqlSchema);
        });
});

builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options
        .UseSqlServer(dbConnectionString, sqlOptions =>
        {
            sqlOptions.MigrationsHistoryTable("EFMigrationHistory", AuthDbContext.SqlSchema);
        });
});

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options => {
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;

        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
    
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
    });


builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfiles));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMomentRepository, MomentRepository>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

var app = builder.Build();
// app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL("/graphql");

app.MapControllers();

app.Run();