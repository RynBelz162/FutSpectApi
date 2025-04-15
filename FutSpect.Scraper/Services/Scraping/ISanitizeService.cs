namespace FutSpect.Scraper.Services.Scraping;

public interface ISanitizeService
{
    string Sanitize(string? value);
    string ToTitleCase(string? value);
}