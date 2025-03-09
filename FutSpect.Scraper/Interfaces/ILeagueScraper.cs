using Microsoft.Playwright;

namespace FutSpect.Scraper.Interfaces;

public interface ILeagueScraper
{
    Task Scrape(IBrowser browser);
}