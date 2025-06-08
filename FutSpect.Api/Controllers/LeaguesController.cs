using Microsoft.AspNetCore.Mvc;
using FutSpect.Shared.Models.Leagues;
using FutSpect.Api.Services.Leagues;
using FutSpect.Api.Models;
using FutSpect.Shared.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

    [Tags("Leagues")]
    [HttpGet]
    [EndpointSummary("Retrieve all Leagues")]
    [EndpointDescription("Returns a list of all leagues.")]
    [ProducesResponseType(typeof(Paged<League>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Paged<League>>> GetLeagues(
        [FromQuery]
        [MaxLength(100)]
        [Description("The name of the league to filter by (optional).")]
        string? searchTerm,

        [FromQuery]
        [Range(1, 1000)]
        [DefaultValue(1)]
        [Description("The page number requested (optional).")]
        int page,

        [FromQuery]
        [Range(5, 25)]
        [DefaultValue(10)]
        [Description("The number of items returned for each page (optional).")]
        int pageSize
    )
    {
        var request = new LeaguesRequest
        {
            SearchTerm = searchTerm,
            Page = page,
            PageSize = pageSize
        };

        var leagues = await _leagueService.Get(request);
        return Ok(leagues);
    }
}
