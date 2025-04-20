using FutSpect.Scraper.Models;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public interface IClubScraper
{
    string LeagueName { get; }
    int CountryId { get; }
    Task<ClubScrapeInfo[]> ScrapeClubs(IBrowserContext browserContext);
}