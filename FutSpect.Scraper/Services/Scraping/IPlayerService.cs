namespace FutSpect.Scraper.Services.Scraping;

public interface IPlayerService
{
    (string? FirstName, string LastName) GetName(string value);
    int GetPositionId(string value);
}