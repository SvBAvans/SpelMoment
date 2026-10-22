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

    
    [HttpPost("{id:int}/registrations")]
    public IActionResult RegisterMoment(int id)
    {
        var identityId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (identityId == null)
            return Unauthorized();

        var user = _userRepository.GetByIdentityId(identityId);

        if (user == null)
            return Unauthorized();

        var moment = _momentRepository.GetById(id);

        if (moment == null)
            return NotFound();

        if (moment.Organiser?.Id == user.Id)
            return Conflict("Je kunt je niet aanmelden voor je eigen spelavond.");

        if (moment.Players.Any(player => player.Id == user.Id))
            return Conflict("Je bent al aangemeld.");

        if (moment.Players.Count >= moment.MaxPlayers)
            return Conflict("Deze spelavond is vol.");

        var age = moment.DateTime.Year - user.Birthday.Year;

        if (user.Birthday > DateOnly.FromDateTime(moment.DateTime).AddYears(-age))
            age--;

        if (moment.Is18Plus && age < 18)
            return BadRequest("Je moet minimaal 18 jaar zijn voor deze spelavond.");

        var alreadyRegisteredThatDay = _momentRepository.GetAll()
            .Any(m =>
                m.Id != moment.Id &&
                m.DateTime.Date == moment.DateTime.Date &&
                m.Players.Any(player => player.Id == user.Id));

        if (alreadyRegisteredThatDay)
            return Conflict("Je bent op deze dag al aangemeld voor een spelavond.");

        moment.Players.Add(user);

        _momentRepository.Update(moment);

        return Created(
            $"/api/moments/{moment.Id}/registrations",
            new SuccessResponse
            {
                Success = true,
                Message = "Je bent aangemeld voor de spelavond."
            });
    }
}