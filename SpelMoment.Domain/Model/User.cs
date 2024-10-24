namespace SpelMoment.Domain.Model;

public class User
{
    public int Id { get; set; }
    public string? IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateOnly Birthday { get; set; }
    public GenderEnum Gender { get; set; }
    public bool HasLactoseIntolerance { get; set; }
    public bool HasNutAllergy { get; set; }
    public bool IsVegetarian { get; set; }
    public bool DrinksAlcohol { get; set; }
    public Address? Address { get; set; }

    public IEnumerable<Moment> RegisteredMoments { get; }
    public IEnumerable<Moment> OrganizedMoments { get; }

    public enum GenderEnum
    {
        Male,
        Female,
        Other
    }
}