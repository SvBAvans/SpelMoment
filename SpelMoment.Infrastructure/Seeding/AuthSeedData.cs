using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SpelMoment.Domain.Model;

namespace SpelMoment.Infrastructure.Seeding;

public class AuthSeedData (ILogger<AuthSeedData> logger, RoleManager<IdentityRole> roleManager)
{
    private readonly ILogger<AuthSeedData> logger = logger;
    private readonly RoleManager<IdentityRole> roleManager = roleManager;
    
    public async Task seedDatabase()
    {
        if (!roleManager.Roles.Any())
        {
            foreach (var role in Enum.GetValues<UserRole>())
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
            
            logger.LogInformation("Roles seeded");
        }
        else
        {
            logger.LogError("Roles already present");
        }
    }
}