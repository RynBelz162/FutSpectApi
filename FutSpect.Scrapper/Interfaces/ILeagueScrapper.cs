using Microsoft.Playwright;

namespace FutSpect.Scrapper.Interfaces;

public interface ILeagueScrapper
{
    Task Scrap(IBrowser browser);
}