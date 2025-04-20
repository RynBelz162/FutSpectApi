namespace FutSpect.Scraper.Services.Leagues;

public interface ILeagueService
{
    Task<int> GetLeagueId(string name, int countryId);
}