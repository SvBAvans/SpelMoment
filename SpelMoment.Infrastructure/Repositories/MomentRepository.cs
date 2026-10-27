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
        if (moment == null) return;
        dbContext.Moments.Remove(moment);
        dbContext.SaveChanges();
    }

    public IEnumerable<Moment> GetAll()
    {
        return dbContext.Moments
            .Include(moment => moment.Organiser)
            .Include(moment => moment.Players)
            .Include(moment => moment.Games)
            .OrderBy(moment => moment.DateTime)
            .ToList();
    }

    public Moment? GetById(int id)
    {
        return dbContext.Moments
            .Include(moment => moment.Organiser)
            .Include(moment => moment.Players)
            .Include(moment => moment.Games)
            .FirstOrDefault(moment => moment.Id == id);
    }

    public void Update(Moment moment)
    {
        var existingGames = dbContext.Games
            .Where(game => game.Moment.Id == moment.Id)
            .Select(game => game.Id)
            .ToHashSet();
        
        var currentGameIds = moment.Games
            .Select(game => game.Id)
            .ToHashSet();

        foreach (var gameId in existingGames.Except(currentGameIds))
        {
            var game = dbContext.Games.Find(gameId);

            if (game != null)
                dbContext.Games.Remove(game);
        }
        
        foreach (var game in moment.Games)
        {
            if (!existingGames.Contains(game.Id))
            {
                dbContext.Entry(game).State = EntityState.Added;
            }
        }

        dbContext.SaveChanges();
    }
}