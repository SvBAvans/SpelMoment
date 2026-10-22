using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.Models;

namespace SpelMoment.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IUserRepository userRepository;
    private readonly IMomentRepository momentRepository;
    
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository, IMomentRepository momentRepository)
    {
        _logger = logger;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.userRepository = userRepository;
        this.momentRepository = momentRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = "")
    {
        var model = new RegisterViewModel() { Role = UserRole.User, ReturnUrl = returnUrl};
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            ModelState.AddModelError("", "You are already signed in.");
        }

        var role = model.Role.ToString();
        if (!roleManager.RoleExistsAsync(role).Result)
        {
            ModelState.AddModelError("", "Specified role does not exist.");
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new IdentityUser()
        {
            UserName = model.Email,
            Email = model.Email
        };

        var registerResult = await userManager.CreateAsync(user, model.Password);

        if (registerResult.Succeeded)
        {
            await userManager.AddClaimAsync(user, new Claim("UserType", "user"));
            await userManager.AddToRoleAsync(user, model.Role.ToString());

            User appUser = new()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = new Address {Street = model.Address.Street, HouseNumber = model.Address.HouseNumber, City = model.Address.City},
                Role = model.Role,
                IdentityId = user.Id,
                Birthday = model.Birthday!.Value,
                Gender = model.Gender!.Value,
                DrinksAlcohol = model.DrinksAlcohol,
                IsVegetarian = model.IsVegetarian,
                HasLactoseIntolerance = model.HasLactoseIntolerance,
                HasNutAllergy = model.HasNutAllergy,
            };
            userRepository.Add(appUser);
            
            return RedirectToAction("Login", new { returnUrl = model.ReturnUrl ?? "/" });
        }

        foreach (var error in registerResult.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel {ReturnUrl = returnUrl});
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid) {
            var user = await userManager.FindByEmailAsync(loginViewModel.Email);
            if (user != null)
            {
                await signInManager.SignOutAsync();
                if ((await signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, false)).Succeeded)
                {
                    return Redirect(loginViewModel.ReturnUrl ?? "/");
                }
            }
        }
        ModelState.AddModelError("", "Invalid email or password.");
        return View();
    }

    public async Task<RedirectResult> LogoutAsync(string returnUrl = "/")
    {
        await signInManager.SignOutAsync();
        return Redirect(returnUrl);
    }
    
    
    [HttpGet]
    public IActionResult Profile()
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = identityId == null
            ? null
            : userRepository.GetByIdentityId(identityId);

        if (user == null)
            return Forbid();

        var model = new ProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Birthday = user.Birthday,
            Street = user.Address!.Street,
            HouseNumber = user.Address!.HouseNumber,
            City = user.Address!.City,
            HasLactoseIntolerance = user.HasLactoseIntolerance,
            HasNutAllergy = user.HasNutAllergy,
            IsVegetarian = user.IsVegetarian,
            WantsNonAlcoholicDrinks = !user.DrinksAlcohol
        };

        return View(model);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Profile(ProfileViewModel model)
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = identityId == null
            ? null
            : userRepository.GetByIdentityId(identityId);

        if (user == null)
            return Forbid();

        if (model.Birthday > DateOnly.FromDateTime(DateTime.Today))
        {
            ModelState.AddModelError(
                nameof(model.Birthday),
                "Je geboortedatum mag niet in de toekomst liggen.");
        }

        if (model.Birthday > DateOnly.FromDateTime(DateTime.Today.AddYears(-16)))
        {
            ModelState.AddModelError(
                nameof(model.Birthday),
                "Je moet minimaal 16 jaar oud zijn.");
        }
        
        if (user.OrganizedMoments?.Any() == true &&
            model.Birthday > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
        {
            ModelState.AddModelError(
                nameof(model.Birthday),
                "Als organisator moet je minimaal 18 jaar oud zijn.");
        }

        if (!ModelState.IsValid)
            return View(model);

        user.FirstName = model.FirstName.Trim();
        user.LastName = model.LastName.Trim();
        user.Birthday = model.Birthday;
        
        user.Address.Street = model.Street.Trim();
        user.Address.HouseNumber = model.HouseNumber.Trim();
        user.Address.City = model.City.Trim();

        user.HasLactoseIntolerance = model.HasLactoseIntolerance;
        user.HasNutAllergy = model.HasNutAllergy;
        user.IsVegetarian = model.IsVegetarian;
        user.DrinksAlcohol = !model.WantsNonAlcoholicDrinks;

        userRepository.Update(user);

        TempData["SuccessMessage"] = "Je profiel is succesvol bijgewerkt.";

        return RedirectToAction(nameof(Profile));
    }
    
    
    [Authorize]
    [HttpGet]
    public IActionResult OrganizedMoments()
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = identityId == null
            ? null
            : userRepository.GetByIdentityId(identityId);

        if (user == null)
            return Forbid();

        var moments = momentRepository.GetAll()
            .Where(moment => moment.Organiser?.Id == user.Id)
            .OrderBy(moment => moment.DateTime)
            .ToList();

        return View(moments);
    }

    [Authorize]
    [HttpGet]
    public IActionResult RegisteredMoments()
    {
        var identityId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = identityId == null
            ? null
            : userRepository.GetByIdentityId(identityId);

        if (user == null)
            return Forbid();

        var moments = momentRepository.GetAll()
            .Where(moment => moment.Players.Any(player => player.Id == user.Id))
            .OrderBy(moment => moment.DateTime)
            .ToList();

        return View(moments);
    }

}