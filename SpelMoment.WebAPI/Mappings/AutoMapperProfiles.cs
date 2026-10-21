using AutoMapper;
using SpelMoment.Domain.Model;
using SpelMoment.WebAPI.Models.DTO;

namespace SpelMoment.WebAPI.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Moment, MomentDto>()
            .ForMember(dest => dest.PlayerIds, opt => opt.MapFrom(src => src.Players.Select(user => user.Id)))
            .ForMember(dest => dest.OrganizerId, opt => opt.MapFrom(src => src.Organiser!.Id))
            .ReverseMap();
        
        CreateMap<Game, GameDto>().ReverseMap();

    }
}