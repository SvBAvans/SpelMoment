using SpelMoment.Domain.Model;

namespace SpelMoment.Domain.Interfaces;

public interface IGameRepository
{
    public IEnumerable<Game> GetAll();
    public void GetById(Guid id);
    public void Add(Game game);
    public void Update(Game game);
    public void Delete(int id);
}