namespace SpelMoment.Domain.Model;

public class Moment
{
    public int Id { get; set; }
    public User Oganiser { get; set; }
    public DateTime DateTime { get; set; }
    public Address Address { get; set; }
    public int maxPlayers { get; set; }
    public IEnumerable<User> Players { get; set; } 
}