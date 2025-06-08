using FutSpect.Dal.Interfaces;
using FutSpect.Dal.Repositories.Leagues;
using FutSpect.Shared.Models;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Api.Services.Leagues;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;

    public LeagueService(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<Paged<League>> Get(ISearchable searchable)
    {
        var results = await _leagueRepository.Get(searchable);

        return new Paged<League>
        {
            Items = results,
            Page = searchable.Page,
            PageSize = searchable.PageSize,
        };
    }
}
