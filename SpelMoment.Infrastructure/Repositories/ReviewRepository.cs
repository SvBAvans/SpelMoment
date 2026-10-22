using Microsoft.EntityFrameworkCore;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.Infrastructure.Data;

namespace SpelMoment.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly SpelMomentDbContext dbContext;

    public ReviewRepository(SpelMomentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IEnumerable<Review> GetByMomentId(int momentId)
    {
        return dbContext.Reviews
            .Include(review => review.Reviewer)
            .Where(review => EF.Property<int>(review, "MomentId") == momentId)
            .ToList();
    }

    public IEnumerable<Review> GetByOrganiserId(int organiserId)
    {
        return dbContext.Reviews
            .Include(review => review.Reviewer)
            .Include(review => review.Moment)
            .Where(review => review.Moment.Organiser != null &&
                             review.Moment.Organiser.Id == organiserId)
            .ToList();
    }

    public void Add(Review review)
    {
        dbContext.Reviews.Add(review);
        dbContext.SaveChanges();
    }
}