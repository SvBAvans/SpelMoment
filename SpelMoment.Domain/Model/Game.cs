namespace SpelMoment.Domain.Model;

public class Game
{
    public string Name { get; set; }
    public string Description { get; set; }
    public GameGenre Genre { get; set; }
    public bool Is18Plus { get; set; }
    public string ImageUrl { get; set; }
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