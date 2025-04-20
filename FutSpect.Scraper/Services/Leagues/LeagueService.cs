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

    public async Task<int> GetLeagueId(string name, int countryId)
    {
        var league = await _leagueRepository.Get(name);
        if (league is not null)
        {
            return league.Id;
        }

        league = new League
        {
            Name = name,
            CountryId = countryId,
        };

        return await _leagueRepository.Save(league);
    }
}