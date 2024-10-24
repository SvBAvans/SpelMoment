using SpelMoment.Domain.Model;

namespace SpelMoment.Domain.Interfaces;

public interface IUserRepository
{
    public IEnumerable<User> GetAll();
    public User? GetById(int id);
    public User? GetByIdentityId(string identityId);
    public void Add(User user);
    public void Update(User user);
    public void Delete(int id);
}