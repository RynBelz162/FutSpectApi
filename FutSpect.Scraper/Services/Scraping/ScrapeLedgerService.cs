using FutSpect.DAL.Repositories.Leagues;
using FutSpect.DAL.Repositories.Scraping;
using FutSpect.Shared.Models.Scraping;

namespace FutSpect.Scraper.Services.Scraping;

public class ScrapeLedgerService : IScrapeLedgerService
{
    private readonly IScrapeLedgerRepository _scrapeLedgerRepository;
    private readonly ILeagueRepository _leagueRepository;

    public ScrapeLedgerService(IScrapeLedgerRepository scrapeLedgerRepository, ILeagueRepository leagueRepository)
    {
        _scrapeLedgerRepository = scrapeLedgerRepository;
        _leagueRepository = leagueRepository;
    }

    public async Task Add(string league, int countryId, int typeId)
    {
        var leagueId = await _leagueRepository.Get(league, countryId);
        var scrapeLedger = new ScrapeLedger
        {
            LeagueId = leagueId,
            TypeId = typeId
        };

        await _scrapeLedgerRepository.Add(scrapeLedger);
    }

    public async Task<bool> Any(string league, int countryId, int typeId, DateTime createdDate)
    {
        return await _scrapeLedgerRepository.Any(league, countryId, typeId, createdDate);
    }
}