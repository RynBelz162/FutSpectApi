using Microsoft.AspNetCore.Mvc;
using FutSpect.Shared.Models.Leagues;
using FutSpect.Api.Services.Leagues;
using FutSpect.Api.Models;
using FutSpect.Shared.Models;

namespace FutSpect.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly ILeagueService _leagueService;

    public LeaguesController(ILeagueService leagueService)
    {
        _leagueService = leagueService;
    }

    [HttpGet]
    [EndpointSummary("Retrieve all leagues")]
    [EndpointDescription("Returns a list of all leagues.")]
    [Tags("Leagues")]
    public async Task<ActionResult<Paged<League>>> GetLeagues([FromQuery] LeaguesRequest request)
    {
        var leagues = await _leagueService.Get(request);
        return Ok(leagues);
    }
}
