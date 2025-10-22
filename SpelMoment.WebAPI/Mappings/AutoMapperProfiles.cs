using AutoMapper;
using SpelMoment.Domain.Model;
using SpelMoment.WebAPI.Models.DTO;
using SpelMoment.WebAPI.Models.Request.Moment;

namespace SpelMoment.WebAPI.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Moment, MomentDto>()
            .ForMember(dest => dest.PlayerIds, opt => opt.MapFrom(src => src.Players.Select(user => user.Id)))
            .ForMember(dest => dest.OrganizerId, opt => opt.MapFrom(src => src.Organiser.Id))
            .ReverseMap();

        CreateMap<CreateMomentRequest, Moment>();
        
        CreateMap<Game, GameDto>().ReverseMap();
        
        CreateMap<UpdateMomentRequest, Moment>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}