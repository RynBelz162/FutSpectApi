using FutSpect.Scraper.Models;
using FutSpect.Shared.Models.Leagues;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public interface IClubScraper
{
    League League { get; }
    Task<ClubScrapeInfo[]> ScrapeClubs(IBrowserContext browserContext);
}