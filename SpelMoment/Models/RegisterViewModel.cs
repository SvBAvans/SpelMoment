using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SpelMoment.Domain.Model;

namespace SpelMoment.Models;

public class RegisterViewModel
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [PasswordPropertyText]
    public string Password { get; set; } = null!;

    [Required]
    [PasswordPropertyText]
    public string PasswordConfirmation { get; set; } = null!;

    [Required]
    public DateOnly Birthday { get; set; }
    [Required]
    public User.GenderEnum Gender { get; set; }
    [Required]
    public bool HasLactoseIntolerance { get; set; }
    [Required]
    public bool HasNutAllergy { get; set; }
    [Required]
    public bool IsVegetarian { get; set; }
    [Required]
    public bool DrinksAlcohol { get; set; }
    
    [Required]
    public Address Address { get; set; } = null!;

    public string? ReturnUrl { get; set; }
    
    public required UserRole Role { get; set; }
}