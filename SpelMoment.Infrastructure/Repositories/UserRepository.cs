using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.Infrastructure.Data;

namespace SpelMoment.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private SpelMomentDbContext dbContext;

    public UserRepository(SpelMomentDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public IEnumerable<User> GetAll()
    {
        return dbContext.Users.ToList();
    }

    public User? GetById(int id)
    {
        return dbContext.Users.Find(id);
    }

    public void Add(User user)
    {
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }

    public void Delete(int id)
    {
        var user = dbContext.Users.Find(id);
        if (user != null)
        {
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }
    }
}