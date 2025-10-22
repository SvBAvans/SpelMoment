using SpelMoment.Domain.Model;
using SpelMoment.WebAPI.Models.DTO;

namespace SpelMoment.WebAPI.Models.Request.Moment
{
    public class UpdateMomentRequest
    {
        public DateTime DateTime { get; set; }
        public Address Address { get; set; } = null!;
        public bool IsLactoseIntoleranceFriendly { get; set; }
        public bool IsVegetarianFriendly { get; set; }
        public bool IsNutAllergyFriendly { get; set; }
        public bool IsNonAlcoholicFriendly { get; set; }
        public IEnumerable<GameDto> Games { get; set; } = null!;
    }
}