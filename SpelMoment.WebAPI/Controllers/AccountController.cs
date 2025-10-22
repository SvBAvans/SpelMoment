using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.WebAPI.Models.Request;

namespace SpelMoment.WebAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class AccountController
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly IUserRepository userRepository;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.userRepository = userRepository;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccessTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> Login([FromBody] LoginRequest loginRequest, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies)
    {
        var useCookieScheme = (useCookies == true) || (useSessionCookies == true);
        var isPersistent = (useCookies == true) && (useSessionCookies != true);
        signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, isPersistent,
            lockoutOnFailure: false);
        
        if (!result.Succeeded)
        {
            return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
        }
        
        return TypedResults.Empty;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Results<Created, ValidationProblem, ProblemHttpResult>> Register([FromBody] RegisterRequest registerRequest)
    {
        var emailAddressAttribute = new EmailAddressAttribute();
        if (string.IsNullOrEmpty(registerRequest.Email) || !emailAddressAttribute.IsValid(registerRequest.Email))
        {
            // return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var user = new IdentityUser()
        {
            UserName = registerRequest.Email,
            Email = registerRequest.Email
        };

        var registerResult = await userManager.CreateAsync(user, registerRequest.Password);
        if (registerResult.Succeeded)
        {
            await userManager.AddClaimAsync(user, new Claim("UserType", "user"));
            await userManager.AddToRoleAsync(user, registerRequest.Role.ToString());
            
            
            User appUser = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Address = new Address {Street = registerRequest.Address.Street, HouseNumber = registerRequest.Address.HouseNumber, City = registerRequest.Address.City},
                Role = registerRequest.Role,
                IdentityId = user.Id,
                Birthday = new DateOnly(),
                Gender = registerRequest.Gender,
                DrinksAlcohol = registerRequest.DrinksAlcohol,
                IsVegetarian = registerRequest.IsVegetarian,
                HasLactoseIntolerance = registerRequest.HasLactoseIntolerance,
                HasNutAllergy = registerRequest.HasNutAllergy,
            };
            userRepository.Add(appUser);

            return TypedResults.Created();
        }

        return TypedResults.Problem(registerResult.ToString(), statusCode: StatusCodes.Status400BadRequest);
    }
}