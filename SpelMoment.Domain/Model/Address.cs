using Microsoft.EntityFrameworkCore;

namespace SpelMoment.Domain.Model;

[Owned]
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string HouseNumber { get; set; }
}