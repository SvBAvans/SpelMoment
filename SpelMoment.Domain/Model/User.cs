namespace SpelMoment.Domain.Model;

public class User
{
    public int Id { get; set; }
    public string? IdentityId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public DateOnly Birthday { get; set; }
    public GenderEnum Gender { get; set; }
    public bool HasLactoseIntolerance { get; set; }
    public bool HasNutAllergy { get; set; }
    public bool IsVegetarian { get; set; }
    public bool DrinksAlcohol { get; set; }
    public Address? Address { get; set; }

    public IEnumerable<Moment> RegisteredMoments { get; } = null!;
    public IEnumerable<Moment> OrganizedMoments { get; } = null!;

    public enum GenderEnum
    {
        Male,
        Female,
        Other
    }
    
    public String ToDisplayString()
    {
        return $"{FirstName} {LastName}";
    }
}