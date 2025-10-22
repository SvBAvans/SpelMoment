using Microsoft.EntityFrameworkCore;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.Infrastructure.Data;

namespace SpelMoment.Infrastructure.Repositories;

public class MomentRepository : IMomentRepository
{
    private SpelMomentDbContext dbContext;

    public MomentRepository(SpelMomentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public void Add(Moment moment)
    {
        dbContext.Moments.Add(moment);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var moment = dbContext.Moments.Find(id);
        if (moment != null)
        {
            dbContext.Moments.Remove(moment);
            dbContext.SaveChanges();
        }
    }

    public IEnumerable<Moment> GetAll()
    {
        return dbContext.Moments.Include(moment => moment.Organiser).Include(moment => moment.Players).ToList();
    }

    public Moment? GetById(int id)
    {
        return dbContext.Moments.Include(moment => moment.Organiser)
            .Include(moment => moment.Players).FirstOrDefault(moment => moment.Id == id);
    }

    public void Update(Moment moment)
    {
        dbContext.Update(moment);
        dbContext.SaveChanges();
    }
}