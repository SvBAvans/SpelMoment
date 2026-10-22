using System.ComponentModel.DataAnnotations;

namespace SpelMoment.Models;

public class ProfileViewModel
{
    [Required(ErrorMessage = "Vul je voornaam in.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je achternaam in.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je geboortedatum in.")]
    [DataType(DataType.Date)]
    public DateOnly Birthday { get; set; }

    [Required(ErrorMessage = "Vul je straat in.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je huisnummer in.")]
    public string HouseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul je woonplaats in.")]
    public string City { get; set; } = string.Empty;
    
    public bool HasLactoseIntolerance { get; set; }
    public bool HasNutAllergy { get; set; }
    public bool IsVegetarian { get; set; }
    public bool WantsNonAlcoholicDrinks { get; set; }
}