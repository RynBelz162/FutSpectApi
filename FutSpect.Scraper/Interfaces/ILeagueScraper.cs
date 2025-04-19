using FutSpect.Scraper.Models;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Interfaces;

public interface ILeagueScraper
{
    Task<ClubScrapeInfo[]> ScrapeClubs(IBrowserContext browserContext);
}