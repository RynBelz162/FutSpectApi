namespace FutSpect.Scraper.Services.Scraping;

public interface IScrapeLedgerService
{
    Task AddLeagueLedger(int leagueId);
    Task<bool> Any(string league, int countryId, int typeId, DateTime createdDate);
}