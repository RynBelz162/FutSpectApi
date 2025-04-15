using FutSpect.DAL.Repositories.Leagues;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Scraper.Services.Leagues.Usl;

public class UslLeagueService(ILeagueRepository leagueRepository) : IUslLeagueService
{
    public async Task<int> GetLeagueId()
    {
        var uslLeague = await leagueRepository.Get("United Soccer League");
        if (uslLeague is not null)
        {
            return uslLeague.Id;
        }

        var league = new League
        {
            Name = "United Soccer League",
            CountryId = Countries.USA,
        };

        return await leagueRepository.Save(league);
    }
}