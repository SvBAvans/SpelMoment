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
    private readonly IUserRepository UserRepository;
    
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
    {
        _logger = logger;
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        UserRepository = userRepository;
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
            return View();
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
                Birthday = model.Birthday,
                Gender = model.Gender,
                DrinksAlcohol = model.DrinksAlcohol,
                IsVegetarian = model.IsVegetarian,
                HasLactoseIntolerance = model.HasLactoseIntolerance,
                HasNutAllergy = model.HasNutAllergy,
            };
            UserRepository.Add(appUser);
            
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
    public IActionResult Login(string returnUrl = null)
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
}