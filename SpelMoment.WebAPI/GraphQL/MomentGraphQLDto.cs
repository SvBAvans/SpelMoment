namespace SpelMoment.WebAPI.GraphQL;

public class MomentGraphQLDto
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int MaxPlayers { get; set; }
    public bool Is18Plus { get; set; }
    public List<GameGraphQlDto> Games { get; set; } = new();
}

public class GameGraphQlDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool Is18Plus { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}