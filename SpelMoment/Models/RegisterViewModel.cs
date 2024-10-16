using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SpelMoment.Domain.Model;

namespace SpelMoment.Models;

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
    [Required]
    [PasswordPropertyText]
    public string PasswordConfirmation { get; set; }
    
    public DateOnly Birthday { get; set; }
    public User.GenderEnum Gender { get; set; }
    public bool HasLactoseIntolerance { get; set; }
    public bool HasNutAllergy { get; set; }
    public bool IsVegetarian { get; set; }
    public bool DrinksAlcohol { get; set; }
    
    public string? ReturnUrl { get; set; }
    
    public required UserRole Role { get; set; }
}