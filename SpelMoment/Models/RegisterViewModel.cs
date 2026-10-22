using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SpelMoment.Domain.Model;

namespace SpelMoment.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vul je voornaam in.")]
    [StringLength(100, ErrorMessage = "Je voornaam mag maximaal 100 tekens bevatten.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je achternaam in.")]
    [StringLength(100, ErrorMessage = "Je achternaam mag maximaal 100 tekens bevatten.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je e-mailadres in.")]
    [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul een wachtwoord in.")]
    [MinLength(8, ErrorMessage = "Je wachtwoord moet minimaal 8 tekens bevatten.")]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bevestig je wachtwoord.")]
    [Compare(
        nameof(Password),
        ErrorMessage = "De wachtwoorden komen niet overeen."
    )]
    [PasswordPropertyText]
    public string PasswordConfirmation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je geboortedatum in.")]
    public DateOnly? Birthday { get; set; }

    [Required(ErrorMessage = "Selecteer je geslacht.")]
    public User.GenderEnum? Gender { get; set; }

    public bool HasLactoseIntolerance { get; set; }

    public bool HasNutAllergy { get; set; }

    public bool IsVegetarian { get; set; }

    public bool DrinksAlcohol { get; set; }

    [Required(ErrorMessage = "Vul je adresgegevens in.")]
    public Address Address { get; set; } = new();

    public string? ReturnUrl { get; set; }

    public required UserRole Role { get; set; }
}