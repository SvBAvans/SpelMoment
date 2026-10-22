using SpelMoment.Domain.Model;

namespace SpelMoment.Models;

public class HomeViewModel
{
    public IEnumerable<Moment>? SuggestedMoments { get; set; } = null!;
}