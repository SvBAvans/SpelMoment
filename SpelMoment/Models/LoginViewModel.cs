using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SpelMoment.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [UIHint("Password")]
    [PasswordPropertyText]
    public string Password { get; set; }
    public string? ReturnUrl { get; set; }
}