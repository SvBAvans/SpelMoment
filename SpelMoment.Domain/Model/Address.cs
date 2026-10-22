using Microsoft.EntityFrameworkCore;

namespace SpelMoment.Domain.Model;

[Owned]
public class Address
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string HouseNumber { get; set; } = null!;

    public string ToDisplayString()
    {
        return $"{Street} {HouseNumber}, {City}";
    }
}