using FutSpect.DAL.Repositories.Leagues;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Scraper.Services.Leagues.Mls;

public class MlsLeagueService(ILeagueRepository leagueRepository) : IMlsLeagueService
{
    public async Task<int> GetLeagueId()
    {
        var mlsLeague = await leagueRepository.Get("Major League Soccer");
        if (mlsLeague is not null)
        {
            return mlsLeague.Id;
        }

        var league = new League
        {
            Name = "Major League Soccer",
            CountryId = Countries.USA,
        };

        return await leagueRepository.Save(league);
    }

}