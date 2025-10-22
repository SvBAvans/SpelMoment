using SpelMoment.WebAPI.Models.DTO;

namespace SpelMoment.WebAPI.Models.Response.Moment;

public class CreateMomentResponse
{
    public MomentDto Moment { get; set; } = null!;
}