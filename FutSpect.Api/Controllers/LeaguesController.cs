using Microsoft.AspNetCore.Mvc;
using FutSpect.DAL;
using FutSpect.Shared.Models.Leagues;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LeaguesController : ControllerBase
{
    private readonly FutSpectContext _context;

    public LeaguesController(FutSpectContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<League>>> GetLeagues()
    {
        var leagues = await _context.Leagues.ToListAsync();
        var dtos = leagues.Select(l => new League
        {
            Id = l.Id,
            Name = l.Name,
            Abbreviation = l.Abbreviation,
            CountryId = l.CountryId,
            HasProRel = l.HasProRel,
            PyramidLevel = l.PyramidLevel,
            Website = l.Website
        });
        return Ok(dtos);
    }
}
