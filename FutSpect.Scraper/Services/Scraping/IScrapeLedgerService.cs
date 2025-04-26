namespace FutSpect.Scraper.Services.Scraping;

public interface IScrapeLedgerService
{
    Task Add(string league, int countryId, int typeId);
    Task<bool> Any(string league, int countryId, int typeId, DateTime createdDate);
}