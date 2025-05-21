using FutSpect.Scraper.Models;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public interface ILeagueScraper
{
    Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext);
}