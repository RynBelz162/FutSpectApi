using FutSpect.Shared.Models.Scraping;

namespace FutSpect.DAL.Repositories.Scraping;

public interface IScrapeLedgerRepository
{
    Task Add(ScrapeLedger scrapeLedger);
    Task<bool> Any(int leagueId, int typeId, DateTime createdDate);
}