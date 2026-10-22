using SpelMoment.Domain.Model;

namespace SpelMoment.WebAPI.Models.DTO;

public class GameDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Game.GameGenre Genre { get; set; }
    public bool Is18Plus { get; set; }
    public string ImageUrl { get; set; } = null!;
    public Game.GameType Type { get; set; }
}