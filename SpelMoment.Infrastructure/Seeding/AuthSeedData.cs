using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SpelMoment.Domain.Model;

namespace SpelMoment.Infrastructure.Seeding;

public class AuthSeedData(
    ILogger<AuthSeedData> logger,
    RoleManager<IdentityRole> roleManager,
    UserManager<IdentityUser> userManager)
{
    public async Task SeedDatabase()
    {
        if (roleManager.Roles.Any() || userManager.Users.Any())
        {
            logger.LogInformation("Identity database bevat al gegevens. Seeding overgeslagen.");
            return;
        }

        foreach (var role in Enum.GetValues<UserRole>())
        {
            var result = await roleManager.CreateAsync(
                new IdentityRole(role.ToString()));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        var firstNames = new[]
        {
            "Thomas",
            "Emma",
            "Lucas",
            "Sophie",
            "Noah",
            "Demo"
        };

        foreach (var firstName in firstNames)
        {
            var email = $"{firstName}@pjiesco.com";

            var identityUser = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(identityUser, "test1234");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Gebruiker {email} kon niet worden aangemaakt: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var roleResult = await userManager.AddToRoleAsync(
                identityUser,
                UserRole.User.ToString());

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        logger.LogInformation("Identity database succesvol gevuld.");
    }
}