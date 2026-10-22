using System.ComponentModel.DataAnnotations;
using SpelMoment.Domain.Model;

namespace SpelMoment.Models;

public class CreateMomentViewModel
{
    [Required(ErrorMessage = "Vul een datum en tijd in.")]
    public DateTime? DateTime { get; set; }

    [Required(ErrorMessage = "Vul een straatnaam in.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul een huisnummer in.")]
    public string HouseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul een stad in.")]
    public string City { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "Het aantal spelers moet tussen 1 en 100 liggen.")]
    public int MaxPlayers { get; set; } = 4;

    public bool Is18Plus { get; set; }
    public bool IsLactoseIntoleranceFriendly { get; set; }
    public bool IsVegetarianFriendly { get; set; }
    public bool IsNutAllergyFriendly { get; set; }
    public bool IsNonAlcoholicFriendly { get; set; }

    [MinLength(1, ErrorMessage = "Voeg minimaal één spel toe.")]
    public List<CreateGameViewModel> Games { get; set; } = new();
}

public class CreateGameViewModel
{
    public Guid? Id { get; set; }
    
    [Required(ErrorMessage = "Vul een spelnaam in.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vul een beschrijving in.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecteer een genre.")]
    public Game.GameGenre? Genre { get; set; }

    [Required(ErrorMessage = "Selecteer een speltype.")]
    public Game.GameType? Type { get; set; }

    public bool Is18Plus { get; set; }

    [Required(ErrorMessage = "Vul een afbeeldings-URL in.")]
    [Url(ErrorMessage = "Vul een geldige URL in.")]
    public string ImageUrl { get; set; } = string.Empty;
}