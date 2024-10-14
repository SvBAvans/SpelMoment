using SpelMoment.Domain.Model;

namespace SpelMoment.Domain.Interfaces;

public interface IAddressRepository
{
    public IEnumerable<Address> GetAll();
    public void GetById(Guid id);
    public void Add(Address address);
    public void Update(Address address);
    public void Delete(int id);
}