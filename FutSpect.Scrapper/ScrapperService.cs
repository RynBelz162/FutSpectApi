using FutSpect.Scrapper.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;

namespace FutSpect.Scrapper;

public class ScrapperService(IEnumerable<ILeagueScrapper> leagueScrapers) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

        var scrapTasks = leagueScrapers.Select(scrapper => scrapper.Scrap(browser));

        await Task.WhenAll(scrapTasks);

        await browser.CloseAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}