namespace FutSpect.Scraper.Services.Scraping;

public interface IPlayerInfoParseService
{
    (string? FirstName, string LastName) GetName(string? value);
    int GetPositionId(string? value);
    short GetNumber(string? value);
}