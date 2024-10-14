namespace SpelMoment.Domain.Model;

public class Moment
{
    public int Id { get; set; }
    public User Oganiser { get; set; }
    public DateTime DateTime { get; set; }
    public Address Address { get; set; }
    public int maxPlayers { get; set; }
    public bool IsLactoseIntoleranceFriendly { get; set; }
    public bool IsVegetarianFriendly { get; set; }
    public bool IsNutAllergyFriendly { get; set; }
    public bool IsNonAlcoholicFriendly { get; set; }
    public IEnumerable<User> Players { get; set; } 
    public IEnumerable<Game> Games { get; set; }
    
}