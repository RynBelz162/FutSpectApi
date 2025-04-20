namespace FutSpect.Scraper.Services.Scraping;

public interface IScrapeLedgerService
{
    Task AddLeagueLedger(int leagueId);
    Task<bool> Any(int leagueId, int typeId, DateTime createdDate);
}