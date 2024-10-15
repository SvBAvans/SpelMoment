namespace SpelMoment.Domain.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateOnly Birthday { get; set; }
    public GenderEnum Gender { get; set; }
    public bool HasLactoseIntolerance { get; set; }
    public bool HasNutAllergy { get; set; }
    public bool IsVegetarian { get; set; }
    public bool DrinksAlcohol { get; set; }
    // public string Street { get; set; }
    // public string City { get; set; }
    // public string HouseNumber { get; set; }
    public Address Address { get; set; }

    public IEnumerable<Moment> RegisteredMoments { get; }
    public IEnumerable<Moment> OrganizedMoments { get; }

    public enum GenderEnum
    {
        Male,
        Female,
        Other
    }
}