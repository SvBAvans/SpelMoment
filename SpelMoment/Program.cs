using Microsoft.EntityFrameworkCore;
using SpelMoment.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var dbConnectionString = builder.Configuration.GetValue<string>("DB_CONNECTION_STRING");

builder.Services.AddDbContext<SpelMomentDbContext>(options =>
{
    options
        .UseSqlServer(dbConnectionString, sqlOptions =>
        {
            sqlOptions.MigrationsHistoryTable("EFMigrationHistory", SpelMomentDbContext.SqlSchema);
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{culture=nl}/{controller=Home}/{action=Index}/{id?}",
    constraints: new {culture = "nl|en"});

app.Run();