namespace SpelMoment.Domain.Model;

public class Moment
{
    public int Id { get; set; }
    public User? Organiser { get; set; }
    public DateTime DateTime { get; set; }
    public Address Address { get; set; } = null!;

    public bool Is18Plus { get; set; }
    public bool IsLactoseIntoleranceFriendly { get; set; }
    public bool IsVegetarianFriendly { get; set; }
    public bool IsNutAllergyFriendly { get; set; }
    public bool IsNonAlcoholicFriendly { get; set; }

    public int MaxPlayers { get; set; }

    public ICollection<User> Players { get; set; } = new List<User>();
    public ICollection<Game> Games { get; set; } = new List<Game>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}