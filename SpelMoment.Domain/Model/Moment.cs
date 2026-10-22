namespace SpelMoment.Domain.Model;

public class Moment
{
    public int Id { get; set; }
    public User? Organiser { get; set; }
    public DateTime DateTime { get; set; }
    // public string City { get; set; }
    // public string HouseNumber { get; set; }
    // public int maxPlayers { get; set; }
    public Address Address { get; set; } = null!;
    public bool IsLactoseIntoleranceFriendly { get; set; }
    public bool IsVegetarianFriendly { get; set; }
    public bool IsNutAllergyFriendly { get; set; }
    public bool IsNonAlcoholicFriendly { get; set; }
    public int MaxPlayers { get; set; }
    public IEnumerable<User> Players { get; set; } = null!;
    public IEnumerable<Game> Games { get; set; }
}