using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.WebAPI.Models.Request;

namespace SpelMoment.WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<IdentityUser> signInManager;

    public AccountController(SignInManager<IdentityUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<Results<EmptyHttpResult, UnauthorizedHttpResult>> Login(
        [FromBody] LoginRequest request)
    {
        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Empty;
    }
}