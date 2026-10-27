using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpelMoment.Domain.Model;
using SpelMoment.Infrastructure.Data;

namespace SpelMoment.Infrastructure.Seeding;

public class SpelMomentSeedData(
    SpelMomentDbContext dbContext,
    UserManager<IdentityUser> userManager,
    ILogger<SpelMomentSeedData> logger)
{
    public async Task SeedDatabase()
    {
        if (await dbContext.Users.AnyAsync() ||
            await dbContext.Moments.AnyAsync() ||
            await dbContext.Games.AnyAsync() ||
            await dbContext.Reviews.AnyAsync())
        {
            logger.LogInformation(
                "Domeindatabase bevat al gegevens. Seeding overgeslagen.");

            return;
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

        var identities = new Dictionary<string, IdentityUser>();

        foreach (var firstName in firstNames)
        {
            var email = $"{firstName}@pjiesco.com";

            var identity = await userManager.FindByEmailAsync(email);

            if (identity == null)
            {
                logger.LogWarning(
                    "Identity-account {Email} ontbreekt. Seeding overgeslagen.",
                    email);

                return;
            }

            identities[firstName] = identity;
        }

        // Gebruikers

        var thomas = new User
        {
            IdentityId = identities["Thomas"].Id,
            FirstName = "Thomas",
            LastName = "Jansen",
            Email = "Thomas@pjiesco.com",
            Role = UserRole.User,
            Birthday = new DateOnly(1995, 5, 12),
            Gender = User.GenderEnum.Male,
            DrinksAlcohol = true,
            Address = new Address
            {
                Street = "Stationsstraat",
                HouseNumber = "12",
                City = "Tilburg"
            }
        };

        var emma = new User
        {
            IdentityId = identities["Emma"].Id,
            FirstName = "Emma",
            LastName = "De Vries",
            Email = "Emma@pjiesco.com",
            Role = UserRole.User,
            Birthday = new DateOnly(2000, 8, 21),
            Gender = User.GenderEnum.Female,
            IsVegetarian = true,
            DrinksAlcohol = false,
            Address = new Address
            {
                Street = "Heuvelstraat",
                HouseNumber = "25",
                City = "Tilburg"
            }
        };

        var lucas = new User
        {
            IdentityId = identities["Lucas"].Id,
            FirstName = "Lucas",
            LastName = "Bakker",
            Email = "Lucas@pjiesco.com",
            Role = UserRole.User,
            Birthday = new DateOnly(1998, 3, 14),
            Gender = User.GenderEnum.Male,
            HasLactoseIntolerance = true,
            DrinksAlcohol = true,
            Address = new Address
            {
                Street = "Spoorlaan",
                HouseNumber = "100",
                City = "Tilburg"
            }
        };

        var sophie = new User
        {
            IdentityId = identities["Sophie"].Id,
            FirstName = "Sophie",
            LastName = "Visser",
            Email = "Sophie@pjiesco.com",
            Role = UserRole.User,
            Birthday = new DateOnly(2002, 11, 6),
            Gender = User.GenderEnum.Female,
            HasNutAllergy = true,
            DrinksAlcohol = false,
            Address = new Address
            {
                Street = "Korte Heuvel",
                HouseNumber = "8",
                City = "Tilburg"
            }
        };

        var noah = new User
        {
            IdentityId = identities["Noah"].Id,
            FirstName = "Noah",
            LastName = "Smit",
            Email = "Noah@pjiesco.com",
            Role = UserRole.User,
            Birthday = new DateOnly(1999, 7, 30),
            Gender = User.GenderEnum.Male,
            DrinksAlcohol = true,
            Address = new Address
            {
                Street = "Westermarkt",
                HouseNumber = "42",
                City = "Tilburg"
            }
        };
        
        var demo = new User
        {
            IdentityId = identities["Demo"].Id,
            FirstName = "Demo",
            LastName = "Speler",
            Email = "Demo@pjiesco.com",
            Role = UserRole.User,
            Birthday = new DateOnly(2008, 6, 15),
            Gender = User.GenderEnum.Other,

            HasLactoseIntolerance = true,
            HasNutAllergy = true,
            IsVegetarian = true,
            DrinksAlcohol = false,

            Address = new Address
            {
                Street = "DemoStraat",
                HouseNumber = "1",
                City = "Tilburg"
            }
        };

        dbContext.Users.AddRange(thomas, emma, lucas, sophie, noah, demo);

        // Spelavonden

        var moment1 = new Moment
        {
            Organiser = thomas,
            DateTime = DateTime.Today.AddDays(7).AddHours(19),
            Address = new Address
            {
                Street = "Stationsstraat",
                HouseNumber = "12",
                City = "Tilburg"
            },
            MaxPlayers = 6,
            Is18Plus = false,
            IsVegetarianFriendly = true,
            IsLactoseIntoleranceFriendly = true,
            IsNutAllergyFriendly = true,
            IsNonAlcoholicFriendly = true,
            Players = new List<User> { emma, lucas, sophie }
        };

        var moment2 = new Moment
        {
            Organiser = emma,
            DateTime = DateTime.Today.AddDays(14).AddHours(20),

            Address = new Address
            {
                Street = "Heuvelstraat",
                HouseNumber = "25",
                City = "Tilburg"
            },

            MaxPlayers = 4,
            Is18Plus = true,

            IsVegetarianFriendly = false,
            IsLactoseIntoleranceFriendly = false,
            IsNutAllergyFriendly = false,
            IsNonAlcoholicFriendly = false,

            Players = new List<User>
            {
                thomas,
                noah
            }
        };

        var moment3 = new Moment
        {
            Organiser = lucas,
            DateTime = DateTime.Today.AddDays(21).AddHours(19),
            Address = new Address
            {
                Street = "Spoorlaan",
                HouseNumber = "100",
                City = "Tilburg"
            },
            MaxPlayers = 8,
            Is18Plus = false,
            IsLactoseIntoleranceFriendly = true,
            IsNutAllergyFriendly = true,
            IsNonAlcoholicFriendly = true,
            Players = new List<User> { emma }
        };

        var moment4 = new Moment
        {
            Organiser = sophie,
            DateTime = DateTime.Today.AddDays(-14).AddHours(19),

            Address = new Address
            {
                Street = "Korte Heuvel",
                HouseNumber = "8",
                City = "Tilburg"
            },

            MaxPlayers = 6,
            Is18Plus = false,

            IsVegetarianFriendly = true,
            IsNutAllergyFriendly = true,
            IsNonAlcoholicFriendly = true,

            Players = new List<User>
            {
                thomas,
                emma,
                noah,
                demo
            }
        };

        var moment5 = new Moment
        {
            Organiser = noah,
            DateTime = DateTime.Today.AddDays(-7).AddHours(20),
            Address = new Address
            {
                Street = "Westermarkt",
                HouseNumber = "42",
                City = "Tilburg"
            },
            MaxPlayers = 5,
            Is18Plus = true,
            IsLactoseIntoleranceFriendly = true,
            Players = new List<User> { thomas, lucas, sophie }
        };

        dbContext.Moments.AddRange(
            moment1,
            moment2,
            moment3,
            moment4,
            moment5);

        // Spellen

        dbContext.Games.AddRange(
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Catan",
                Description = "Verzamel grondstoffen en bouw nederzettingen.",
                Genre = Game.GameGenre.Family,
                Type = Game.GameType.Board,
                Is18Plus = false,
                ImageUrl = "https://d36a5wyl9ttzok.cloudfront.net/680x680/b2c/source_images/Catan---Big-Box-2023_L.png",
                Moment = moment1
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Ticket to Ride",
                Description = "Verbind steden met treinroutes.",
                Genre = Game.GameGenre.Family,
                Type = Game.GameType.Board,
                Is18Plus = false,
                ImageUrl = "https://m.media-amazon.com/images/I/91IZNnP7GSL.jpg",
                Moment = moment1
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Cards Against Humanity",
                Description = "Een humoristisch kaartspel voor volwassenen.",
                Genre = Game.GameGenre.Party,
                Type = Game.GameType.Card,
                Is18Plus = true,
                ImageUrl = "https://m.media-amazon.com/images/I/6145Fx5tHBL.jpg",
                Moment = moment2
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Exploding Kittens",
                Description = "Een chaotisch kaartspel vol verrassingen.",
                Genre = Game.GameGenre.Party,
                Type = Game.GameType.Card,
                Is18Plus = false,
                ImageUrl = "https://m.media-amazon.com/images/I/719Tx5F4mfL._AC_UF894,1000_QL80_.jpg",
                Moment = moment2
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Carcassonne",
                Description = "Bouw een landschap met tegels.",
                Genre = Game.GameGenre.Family,
                Type = Game.GameType.Board,
                Is18Plus = false,
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQEUHGhT4WuRwLagb7uKIfeGz3aS4rjeHZmWupJ8fYe6I_aybiAYY1RpoY&s=10",
                Moment = moment3
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "30 Seconds",
                Description = "Omschrijf zoveel mogelijk woorden binnen de tijd.",
                Genre = Game.GameGenre.Party,
                Type = Game.GameType.Board,
                Is18Plus = false,
                ImageUrl = "https://d36a5wyl9ttzok.cloudfront.net/680x680/b2c/source_images/30-Seconds.png",
                Moment = moment3
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Uno",
                Description = "Raak als eerste al je kaarten kwijt.",
                Genre = Game.GameGenre.Family,
                Type = Game.GameType.Card,
                Is18Plus = false,
                ImageUrl = "https://media.s-bol.com/x3K8v9YEq2j9/321kYNO/539x840.jpg",
                Moment = moment4
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Poker",
                Description = "Een klassiek kaartspel.",
                Genre = Game.GameGenre.Party,
                Type = Game.GameType.Card,
                Is18Plus = true,
                ImageUrl = "https://m.media-amazon.com/images/I/81uI0GG0rlL._AC_UF894,1000_QL80_.jpg",
                Moment = moment5
            }
        );

        // Reviews voor afgelopen spelavonden

        dbContext.Reviews.AddRange(
            new Review
            {
                Rating = 5,
                Text = "Supergezellige avond en alles was goed geregeld!",
                Reviewer = thomas,
                Moment = moment4
            },
            new Review
            {
                Rating = 4,
                Text = "Leuke spellen en een fijne sfeer.",
                Reviewer = emma,
                Moment = moment4
            },
            new Review
            {
                Rating = 5,
                Text = "Erg leuke avond, zeker voor herhaling vatbaar.",
                Reviewer = lucas,
                Moment = moment5
            },
            new Review
            {
                Rating = 4,
                Text = "Goede organisatie en gezellige deelnemers.",
                Reviewer = sophie,
                Moment = moment5
            }
        );

        await dbContext.SaveChangesAsync();

        logger.LogInformation(
            "Domeindatabase seeded met 5 gebruikers, 5 spelavonden, 8 spellen en 4 reviews.");
    }
}