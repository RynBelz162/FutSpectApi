using FutSpect.Scraper.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;

namespace FutSpect.Scraper;

public class ScraperService(IEnumerable<ILeagueScraper> leagueScrapers) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });

        var scrapeTasks = leagueScrapers.Select(async scraper =>
        {
            var context = await browser.NewContextAsync(new ()
            {
                UserAgent = Constants.UserAgents.GetRandom()
            });

            _ = await scraper.ScrapeClubs(context);
            await context.CloseAsync();
        });

        await Task.WhenAll(scrapeTasks);

        await browser.CloseAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}