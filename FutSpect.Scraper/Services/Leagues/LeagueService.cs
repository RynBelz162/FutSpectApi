using FutSpect.DAL.Repositories.Leagues;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Scraper.Services.Leagues;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;

    public LeagueService(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public async Task<int> GetOrSave(League league)
    {
        var leagueId = await _leagueRepository.Find(league.Name, league.CountryId);
        if (leagueId is not null)
        {
            return leagueId.Value;
        }

        return await _leagueRepository.Save(league);
    }
}