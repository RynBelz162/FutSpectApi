using FutSpect.Scraper.Models;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public interface ILeagueScraper
{
    string LeagueName { get; }
    int CountryId { get; }
    Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext);
}