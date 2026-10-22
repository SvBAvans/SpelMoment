using SpelMoment.Domain.Model;

namespace SpelMoment.Domain.Interfaces;

public interface IReviewRepository
{
    IEnumerable<Review> GetByMomentId(int momentId);

    IEnumerable<Review> GetByOrganiserId(int organiserId);

    void Add(Review review);
}