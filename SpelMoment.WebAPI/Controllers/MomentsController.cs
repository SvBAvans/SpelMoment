using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.WebAPI.Models.DTO;
using SpelMoment.WebAPI.Models.Request.Moment;
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

        IEnumerable<Moment> moments = _momentRepository.GetAll();
        
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
    
    [HttpPost]
    [ProducesResponseType(typeof(CreateMomentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Created<CreateMomentResponse>, EmptyHttpResult, ProblemHttpResult, BadRequest, UnauthorizedHttpResult>> CreateMoment([FromBody] CreateMomentRequest createMomentRequest)
    {
        
        var moment = _mapper.Map<Moment>(createMomentRequest);
            
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? User.FindFirst("sub")?.Value;

        if (userId == null)
        {
            return TypedResults.Unauthorized();
        }

        var organiser = _userRepository.GetByIdentityId(userId);

        if (organiser == null)
            return TypedResults.BadRequest();
        
        moment.Organiser = organiser;
        
        //TODO: Games are saved as is. Check if they already exist? Should they even be created or does the user have to choose pre-created games?
        
        _momentRepository.Add(moment);

        return TypedResults.Created($"/moments/{moment.Id}", new CreateMomentResponse { Moment = _mapper.Map<MomentDto>(moment) });
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(SuccessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<Results<Ok<SuccessResponse>, EmptyHttpResult, ProblemHttpResult, UnauthorizedHttpResult, NotFound, ForbidHttpResult>> DeleteMoment(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? User.FindFirst("sub")?.Value;

        if (userId == null)
        {
            return TypedResults.Unauthorized();
        }
        
        var moment = _momentRepository.GetById(id);
        if (moment == null)
        {
            return TypedResults.NotFound();
        }

        
        if (moment.Organiser?.IdentityId != userId)
        {
            return TypedResults.Forbid();
        }
        
        _momentRepository.Delete(id);

        return TypedResults.Ok(new SuccessResponse() {Success = true});
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MomentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Results<Ok<MomentResponse>, NotFound, ForbidHttpResult, ProblemHttpResult, UnauthorizedHttpResult>> UpdateMoment(
        int id, [FromBody] UpdateMomentRequest updateMomentRequest)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                     ?? User.FindFirst("sub")?.Value;
        if (userId == null)
            return TypedResults.Unauthorized();
        
        var moment = _momentRepository.GetById(id);
        if (moment == null)
            return TypedResults.NotFound();
        
        if (moment.Organiser?.IdentityId != userId)
        {
            return TypedResults.Problem(
                detail: "Only the organiser can update this SpelMoment.",
                statusCode: StatusCodes.Status403Forbidden
            );
        }
        
        _mapper.Map(updateMomentRequest, moment);
        
        _momentRepository.Update(moment);

        return TypedResults.Ok(new MomentResponse
        {
            Moment = _mapper.Map<MomentDto>(moment)
        });
    }
}