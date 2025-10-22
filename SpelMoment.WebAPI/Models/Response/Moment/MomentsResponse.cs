using SpelMoment.WebAPI.Models.DTO;

namespace SpelMoment.WebAPI.Models.Response.Moment;

public class MomentsResponse
{
    public IEnumerable<MomentDto> Moments { get; set; }
}