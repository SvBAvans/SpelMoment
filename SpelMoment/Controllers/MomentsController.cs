using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.Models;
using SpelMoment.ViewModels;

namespace SpelMoment.Controllers;

[Authorize]
public class MomentsController : Controller
{
    private readonly IMomentRepository momentRepository;
    private readonly IUserRepository userRepository;
    private readonly IReviewRepository reviewRepository;

    public MomentsController(IMomentRepository momentRepository, IUserRepository userRepository, IReviewRepository reviewRepository)
    {
        this.momentRepository = momentRepository;
        this.userRepository = userRepository;
        this.reviewRepository = reviewRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        var moments = momentRepository.GetAll();
        return View(moments);
    }

    [HttpGet]

    [AllowAnonymous]
    public IActionResult Details(int id)
    {
        var moment = momentRepository.GetById(id);
        if (moment == null) return NotFound();

        
        var reviews = reviewRepository.GetByMomentId(id).ToList();

        ViewBag.Reviews = reviews;
        
        var warnings = new List<string>();

        if (User.Identity?.IsAuthenticated == true)
        {
            var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = identityId == null
                ? null
                : userRepository.GetByIdentityId(identityId);

            if (user != null)
            {
                if (user.HasLactoseIntolerance && !moment.IsLactoseIntoleranceFriendly)
                    warnings.Add("Er zijn geen lactosevrije opties beschikbaar.");

                if (user.HasNutAllergy && !moment.IsNutAllergyFriendly)
                    warnings.Add("Er zijn geen notenvrije opties beschikbaar.");

                if (user.IsVegetarian && !moment.IsVegetarianFriendly)
                    warnings.Add("Er zijn geen vegetarische opties beschikbaar.");

                if (!user.DrinksAlcohol && !moment.IsNonAlcoholicFriendly)
                    warnings.Add("Er zijn geen alcoholvrije dranken beschikbaar.");
            }
            
            var organiserReviews = reviewRepository
                .GetByOrganiserId(moment.Organiser!.Id)
                .ToList();

            ViewBag.OrganiserAverageRating = organiserReviews.Any()
                ? organiserReviews.Average(review => review.Rating)
                : 0;

            ViewBag.OrganiserReviewCount = organiserReviews.Count;
        
            ViewBag.OrganisedMomentCount = momentRepository.GetAll()
                .Count(m => m.Organiser?.Id == moment.Organiser?.Id
                            && m.Organiser != null);
            
            ViewBag.CanReview =
                user != null &&
                moment.DateTime <= DateTime.Now &&
                moment.Players.Any(player => player.Id == user.Id) &&
                !reviews.Any(review => review.Reviewer.Id == user.Id);
        }

        ViewBag.DietWarnings = warnings;
        
        return View(moment);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateMomentViewModel
        {
            Games = new List<CreateGameViewModel>
            {
                new()
            }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateMomentViewModel model)
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var organiser = identityId == null ? null : userRepository.GetByIdentityId(identityId);

        if (organiser == null)
            return Forbid();

        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - organiser.Birthday.Year;

        if (organiser.Birthday > today.AddYears(-age))
            age--;

        if (age < 18)
            ModelState.AddModelError("", "Je moet minimaal 18 jaar zijn om een spelmoment te organiseren.");

        if (model.DateTime.HasValue && model.DateTime.Value <= DateTime.Now)
            ModelState.AddModelError(nameof(model.DateTime), "Kies een datum en tijd in de toekomst.");

        if (model.Games == null || model.Games.Count == 0)
            ModelState.AddModelError(nameof(model.Games), "Voeg minimaal één bordspel toe.");

        if (!ModelState.IsValid)
        {
            model.Games ??= new List<CreateGameViewModel>();

            if (model.Games.Count == 0)
                model.Games.Add(new CreateGameViewModel());

            return View(model);
        }

        var moment = new Moment
        {
            Organiser = organiser,
            DateTime = model.DateTime!.Value,
            Address = new Address
            {
                Street = model.Street,
                HouseNumber = model.HouseNumber,
                City = model.City
            },
            MaxPlayers = model.MaxPlayers,
            Is18Plus = model.Is18Plus,
            IsLactoseIntoleranceFriendly = model.IsLactoseIntoleranceFriendly,
            IsVegetarianFriendly = model.IsVegetarianFriendly,
            IsNutAllergyFriendly = model.IsNutAllergyFriendly,
            IsNonAlcoholicFriendly = model.IsNonAlcoholicFriendly
        };

        foreach (var gameModel in model.Games)
            moment.Games.Add(new Game
            {
                Id = Guid.NewGuid(),
                Name = gameModel.Name,
                Description = gameModel.Description,
                Genre = gameModel.Genre!.Value,
                Type = gameModel.Type!.Value,
                Is18Plus = gameModel.Is18Plus,
                ImageUrl = gameModel.ImageUrl
            });

        moment.Is18Plus = moment.Is18Plus || moment.Games.Any(game => game.Is18Plus);

        momentRepository.Add(moment);

        TempData["SuccessMessage"] = "Je spelmoment is succesvol aangemaakt.";

        return RedirectToAction("Details", "Moments", new { id = moment.Id });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Join(int id)
    {
        var moment = momentRepository.GetById(id);
        if (moment == null) return NotFound();

        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = identityId == null ? null : userRepository.GetByIdentityId(identityId);
        if (user == null) return Forbid();

        if (moment.Players.Any(player => player.Id == user.Id))
        {
            TempData["ErrorMessage"] = "Je bent al aangemeld voor deze spelavond.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (moment.Players.Count >= moment.MaxPlayers)
        {
            TempData["ErrorMessage"] = "Deze spelavond is vol.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var age = DateTime.Today.Year - user.Birthday.Year;
        if (user.Birthday > DateOnly.FromDateTime(DateTime.Today).AddYears(-age))
            age--;

        if (moment.Is18Plus && age < 18)
        {
            TempData["ErrorMessage"] = "Je moet minimaal 18 jaar zijn voor deze spelavond.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var alreadyRegisteredToday = momentRepository.GetAll()
            .Any(otherMoment =>
                otherMoment.Id != moment.Id &&
                otherMoment.DateTime.Date == moment.DateTime.Date &&
                otherMoment.Players.Any(player => player.Id == user.Id));

        if (alreadyRegisteredToday)
        {
            TempData["ErrorMessage"] = "Je kunt maar aan één spelavond per dag deelnemen.";
            return RedirectToAction(nameof(Details), new { id });
        }

        moment.Players.Add(user);
        momentRepository.Update(moment);

        TempData["SuccessMessage"] = "Je bent aangemeld voor deze spelavond.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public IActionResult Leave(int id)
    {
        var moment = momentRepository.GetById(id);
        if (moment == null) return NotFound();

        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = identityId == null ? null : userRepository.GetByIdentityId(identityId);
        if (user == null) return Forbid();

        var player = moment.Players.FirstOrDefault(player => player.Id == user.Id);

        if (player == null)
        {
            TempData["ErrorMessage"] = "Je bent niet aangemeld voor deze spelavond.";
            return RedirectToAction(nameof(Details), new { id });
        }

        moment.Players.Remove(player);
        momentRepository.Update(moment);

        TempData["SuccessMessage"] = "Je bent afgemeld voor deze spelavond.";
        return RedirectToAction(nameof(Details), new { id });
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var moment = momentRepository.GetById(id);
        if (moment == null) return NotFound();

        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = identityId == null ? null : userRepository.GetByIdentityId(identityId);

        if (user == null || moment.Organiser?.Id != user.Id)
            return Forbid();

        if (moment.Players.Any())
        {
            TempData["ErrorMessage"] =
                "Dit spelmoment kan niet meer worden bewerkt omdat er deelnemers zijn aangemeld.";
            return RedirectToAction(nameof(Details), new { id });
        }

        var model = new CreateMomentViewModel
        {
            DateTime = moment.DateTime,
            Street = moment.Address.Street,
            HouseNumber = moment.Address.HouseNumber,
            City = moment.Address.City,
            MaxPlayers = moment.MaxPlayers,
            Is18Plus = moment.Is18Plus,
            IsLactoseIntoleranceFriendly = moment.IsLactoseIntoleranceFriendly,
            IsVegetarianFriendly = moment.IsVegetarianFriendly,
            IsNutAllergyFriendly = moment.IsNutAllergyFriendly,
            IsNonAlcoholicFriendly = moment.IsNonAlcoholicFriendly,

            Games = moment.Games.Select(game => new CreateGameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Genre = game.Genre,
                Type = game.Type,
                Is18Plus = game.Is18Plus,
                ImageUrl = game.ImageUrl
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, CreateMomentViewModel model)
    {
        var moment = momentRepository.GetById(id);
        if (moment == null) return NotFound();

        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = identityId == null ? null : userRepository.GetByIdentityId(identityId);

        if (user == null || moment.Organiser?.Id != user.Id)
            return Forbid();

        if (moment.Players.Any())
        {
            TempData["ErrorMessage"] =
                "Dit spelmoment kan niet meer worden bewerkt omdat er deelnemers zijn aangemeld.";
            return RedirectToAction(nameof(Details), new { id });
        }

        if (model.DateTime.HasValue && model.DateTime.Value <= DateTime.Now)
            ModelState.AddModelError(nameof(model.DateTime), "Kies een datum en tijd in de toekomst.");

        if (model.Games == null || model.Games.Count == 0)
            ModelState.AddModelError(nameof(model.Games), "Voeg minimaal één bordspel toe.");

        if (!ModelState.IsValid)
        {
            model.Games ??= new List<CreateGameViewModel>();
            if (model.Games.Count == 0) model.Games.Add(new CreateGameViewModel());

            return View(model);
        }

        moment.DateTime = model.DateTime!.Value;
        moment.Address.Street = model.Street;
        moment.Address.HouseNumber = model.HouseNumber;
        moment.Address.City = model.City;
        moment.MaxPlayers = model.MaxPlayers;

        moment.IsLactoseIntoleranceFriendly = model.IsLactoseIntoleranceFriendly;
        moment.IsVegetarianFriendly = model.IsVegetarianFriendly;
        moment.IsNutAllergyFriendly = model.IsNutAllergyFriendly;
        moment.IsNonAlcoholicFriendly = model.IsNonAlcoholicFriendly;

        
        var submittedGameIds = model.Games
            .Where(game => game.Id.HasValue)
            .Select(game => game.Id!.Value)
            .ToHashSet();
        
        var removedGames = moment.Games
            .Where(game => !submittedGameIds.Contains(game.Id))
            .ToList();

        foreach (var game in removedGames)
        {
            moment.Games.Remove(game);
        }
        
        foreach (var gameModel in model.Games)
        {
            Game? game = null;

            if (gameModel.Id.HasValue)
            {
                game = moment.Games.FirstOrDefault(g => g.Id == gameModel.Id.Value);

                if (game == null)
                    return BadRequest("Een opgegeven spel hoort niet bij dit spelmoment.");
            }
            else
            {
                game = new Game
                {
                    Id = Guid.NewGuid(),
                    Moment = moment
                };

                moment.Games.Add(game);
            }

            game.Name = gameModel.Name;
            game.Description = gameModel.Description;
            game.Genre = gameModel.Genre!.Value;
            game.Type = gameModel.Type!.Value;
            game.Is18Plus = gameModel.Is18Plus;
            game.ImageUrl = gameModel.ImageUrl;
        }


        moment.Is18Plus = model.Is18Plus || moment.Games.Any(game => game.Is18Plus);

        momentRepository.Update(moment);

        TempData["SuccessMessage"] = "Je spelmoment is succesvol bijgewerkt.";
        return RedirectToAction(nameof(Details), new { id });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var moment = momentRepository.GetById(id);
        if (moment == null) return NotFound();

        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = identityId == null ? null : userRepository.GetByIdentityId(identityId);

        if (user == null || moment.Organiser?.Id != user.Id)
            return Forbid();

        if (moment.Players.Any())
        {
            TempData["ErrorMessage"] = "Dit spelmoment kan niet worden verwijderd omdat er deelnemers zijn aangemeld.";
            return RedirectToAction(nameof(Details), new { id });
        }

        momentRepository.Delete(id);

        TempData["SuccessMessage"] = "Je spelmoment is succesvol verwijderd.";
        return RedirectToAction(nameof(Index));
    }
    
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReview(CreateReviewViewModel model)
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = identityId == null
            ? null
            : userRepository.GetByIdentityId(identityId);

        if (user == null)
            return Forbid();

        var moment = momentRepository.GetById(model.MomentId);

        if (moment == null)
            return NotFound();

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Vul een geldige score en reviewtekst in.";
            return RedirectToAction("Details", new { id = model.MomentId });
        }

        if (moment.DateTime > DateTime.Now)
        {
            TempData["ErrorMessage"] = "Je kunt pas na afloop een review plaatsen.";
            return RedirectToAction("Details", new { id = model.MomentId });
        }

        if (!moment.Players.Any(player => player.Id == user.Id))
        {
            TempData["ErrorMessage"] = "Alleen deelnemers kunnen een review plaatsen.";
            return RedirectToAction("Details", new { id = model.MomentId });
        }

        if (reviewRepository.GetByMomentId(model.MomentId)
            .Any(review => review.Reviewer.Id == user.Id))
        {
            TempData["ErrorMessage"] = "Je hebt al een review geplaatst.";
            return RedirectToAction("Details", new { id = model.MomentId });
        }

        var review = new Review
        {
            Rating = model.Rating,
            Text = model.Text.Trim(),
            Reviewer = user,
            Moment = moment
        };

        reviewRepository.Add(review);

        TempData["SuccessMessage"] = "Je review is geplaatst!";
        return RedirectToAction("Details", new { id = model.MomentId });
    }
}