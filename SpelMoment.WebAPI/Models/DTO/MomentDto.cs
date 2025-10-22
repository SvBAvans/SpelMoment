using SpelMoment.Domain.Model;

namespace SpelMoment.WebAPI.Models.DTO;

public class MomentDto
{
    public int Id { get; set; }
    public int OrganizerId { get; set; }
    public DateTime DateTime { get; set; }
    public Address Address { get; set; } = null!;
    public bool IsLactoseIntoleranceFriendly { get; set; }
    public bool IsVegetarianFriendly { get; set; }
    public bool IsNutAllergyFriendly { get; set; }
    public bool IsNonAlcoholicFriendly { get; set; }
    public IEnumerable<int> PlayerIds { get; set; } = null!;
    public IEnumerable<Game> Games { get; set; } = null!;
}