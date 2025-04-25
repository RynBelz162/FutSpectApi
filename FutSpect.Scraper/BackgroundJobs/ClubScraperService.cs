using FutSpect.DAL.Constants;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace FutSpect.Scraper.BackgroundJobs;

public class ClubScraperService : BackgroundService
{
    private static DateTime ScrapeInterval => DateTime.UtcNow.Date.AddDays(-14);
    private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(5));

    private readonly IEnumerable<IClubScraper> _clubScrapers;
    private readonly ILogger<ClubScraperService> _logger;
    private readonly IScrapeLedgerService _scrapeLedgerService;

    public ClubScraperService(IEnumerable<IClubScraper> clubScrapers, ILogger<ClubScraperService> logger, IScrapeLedgerService scrapeLedgerService)
    {
        _clubScrapers = clubScrapers;
        _logger = logger;
        _scrapeLedgerService = scrapeLedgerService;
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
                if (await HasAlreadyScraped(scraper, ScrapeInterval))
                {
                    continue;
                }

                await Scrape(browser, scraper);
            }

            await browser.CloseAsync();
            _logger.LogInformation("Daily club scraping finished at: {Time}", DateTimeOffset.Now);
        }
    }

    private async Task<bool> HasAlreadyScraped(IClubScraper scraper, DateTime scrapeInterval) =>
        await _scrapeLedgerService.Any(scraper.League.Name, scraper.League.CountryId, ScrapeTypes.ClubInfo, scrapeInterval);

    private async Task Scrape(IBrowser browser, IClubScraper scraper)
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
}