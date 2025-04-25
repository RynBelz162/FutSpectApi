using FutSpect.DAL.Constants;
using FutSpect.DAL.Repositories.Scraping;
using FutSpect.Shared.Models.Scraping;

namespace FutSpect.Scraper.Services.Scraping;

public class ScrapeLedgerService : IScrapeLedgerService
{
    private readonly IScrapeLedgerRepository _scrapeLedgerRepository;

    public ScrapeLedgerService(IScrapeLedgerRepository scrapeLedgerRepository)
    {
        _scrapeLedgerRepository = scrapeLedgerRepository;
    }

    public async Task AddLeagueLedger(int leagueId)
    {
        var scrapeLedger = new ScrapeLedger
        {
            LeagueId = leagueId,
            TypeId = ScrapeTypes.LeagueInfo
        };

        await _scrapeLedgerRepository.Add(scrapeLedger);
    }

    public async Task<bool> Any(string league, int countryId, int typeId, DateTime createdDate)
    {
        return await _scrapeLedgerRepository.Any(league, countryId, typeId, createdDate);
    }
}