using SpelMoment.Domain.Interfaces;

namespace SpelMoment.WebAPI.GraphQL;

public class Query
{
    public IEnumerable<MomentGraphQLDto> GetMoments(
        [Service] IMomentRepository momentRepository)
    {
        return momentRepository.GetAll()
            .Select(moment => new MomentGraphQLDto
            {
                Id = moment.Id,
                DateTime = moment.DateTime,
                MaxPlayers = moment.MaxPlayers,
                Is18Plus = moment.Is18Plus,
                Games = moment.Games.Select(game => new GameGraphQlDto
                {
                    Name = game.Name,
                    Description = game.Description,
                    Genre = game.Genre.ToString(),
                    Type = game.Type.ToString(),
                    Is18Plus = game.Is18Plus,
                    ImageUrl = game.ImageUrl
                }).ToList()
            })
            .ToList();
    }
}