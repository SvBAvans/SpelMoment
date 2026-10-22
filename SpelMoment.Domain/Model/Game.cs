using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SpelMoment.Domain.Model;

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public GameGenre Genre { get; set; }
    public bool Is18Plus { get; set; }
    public string ImageUrl { get; set; } = null!;
    public GameType Type { get; set; }

    public enum GameType
    {
        Board,
        Card
    }

    public enum GameGenre
    {
        Family,
        Party,
    }
}