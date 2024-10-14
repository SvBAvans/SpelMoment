namespace SpelMoment.Domain.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateOnly Birthday { get; set; }
    public GenderEnum Gender { get; set; }
    public Address Address { get; set; }

    public enum GenderEnum
    {
        Male,
        Female,
        Other
    }
}