using FutSpect.Scraper.Models;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public interface IPlayerScraper
{
    Task<List<PlayerScrapeInfo>> ScrapePlayers(IBrowserContext browserContext, string rosterUrl);
}