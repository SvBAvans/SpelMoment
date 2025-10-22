using System.ComponentModel.DataAnnotations;
using SpelMoment.Domain.Model;

namespace SpelMoment.WebAPI.Models.Request;

public class RegisterRequest
{
    [Required]
    public required string FirstName { get; set; }
    [Required]
    public required string LastName { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
    
    // [Required]
    public required DateOnly Birthday { get; set; }
    [Required]
    public required User.GenderEnum Gender { get; set; }
    [Required]
    public required bool HasLactoseIntolerance { get; set; }
    [Required]
    public required bool HasNutAllergy { get; set; }
    [Required]
    public required bool IsVegetarian { get; set; }
    [Required]
    public required bool DrinksAlcohol { get; set; }
    
    [Required]
    public required Address Address { get; set; }
    [Required]
    public required UserRole Role { get; set; }
}