using SpelMoment.Domain.Model;

namespace SpelMoment.Domain.Interfaces;

public interface IMomentRepository
{
    public IEnumerable<Moment> GetAll();
    public Moment? GetById(int id);
    public void Add(Moment moment);
    public void Update(Moment moment);
    public void Delete(int id);
}