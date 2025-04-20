using FutSpect.Scraper.Scrapers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace FutSpect.Scraper.BackgroundJobs;

public class ClubScraperService : BackgroundService
{
    private readonly PeriodicTimer _timer = new(TimeSpan.FromDays(1));
    private readonly IEnumerable<IClubScraper> _clubScrapers;
    private readonly ILogger<ClubScraperService> _logger;

    public ClubScraperService(IEnumerable<IClubScraper> clubScrapers, ILogger<ClubScraperService> logger)
    {
        _clubScrapers = clubScrapers;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Daily club scraping started at: {Time}", DateTimeOffset.Now);

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

            foreach (var scraper in _clubScrapers)
            {
                var context = await browser.NewContextAsync(new()
                {
                    UserAgent = Constants.UserAgents.GetRandom()
                });

                try
                {
                    await scraper.ScrapeClubs(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while scraping clubs.");
                }
                finally
                {
                    await context.CloseAsync();
                }
            }

            await browser.CloseAsync();
            _logger.LogInformation("Daily club scraping finished at: {Time}", DateTimeOffset.Now);
        }
    }
}