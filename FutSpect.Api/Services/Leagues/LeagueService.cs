using FutSpect.DAL.Repositories.Leagues;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Api.Services.Leagues;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;

    public LeagueService(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<IEnumerable<League>> Get()
    {
        return await _leagueRepository.Get();
    }
}
