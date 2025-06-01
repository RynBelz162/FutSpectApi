using FutSpect.Shared.Models.Scraping;

namespace FutSpect.Dal.Repositories.Scraping;

public interface IScrapeLedgerRepository
{
    Task Add(ScrapeLedger scrapeLedger);
    Task<bool> Any(string name, int countryId, int typeId, DateTime createdDate);
}