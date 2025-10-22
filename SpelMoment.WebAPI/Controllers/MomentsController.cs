using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;
using SpelMoment.WebAPI.Models.DTO;
using SpelMoment.WebAPI.Models.Response;
using SpelMoment.WebAPI.Models.Response.Moment;

namespace SpelMoment.WebAPI.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class MomentsController : ControllerBase
{

    private IMapper _mapper;
    private IMomentRepository _momentRepository;
    private IUserRepository _userRepository;

    public MomentsController(IMapper mapper, IMomentRepository momentRepository, IUserRepository userRepository)
    {
        _mapper = mapper;
        _momentRepository = momentRepository;
        _userRepository = userRepository;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(MomentsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok<MomentsResponse>, EmptyHttpResult, ProblemHttpResult>> Moments()
    {

        var moments = _momentRepository.GetAll();
        
        return TypedResults.Ok(new MomentsResponse {Moments = _mapper.Map<IEnumerable<MomentDto>>(moments)});
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MomentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok<MomentResponse>, EmptyHttpResult, ProblemHttpResult, NotFound>> GetMomentById(int id)
    {

        var moment = _momentRepository.GetById(id);

        if (moment == null)
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(new MomentResponse {Moment = _mapper.Map<MomentDto>(moment)});
    }
}