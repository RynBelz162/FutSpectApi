using FutSpect.DAL.Constants;
using FutSpect.Scraper.Options;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Scraping;
using FutSpect.Scraper.Utility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace FutSpect.Scraper.BackgroundJobs;

public class ClubScraperService : BackgroundService
{
    private readonly CronTimer _timer;
    private readonly IEnumerable<IClubScraper> _clubScrapers;
    private readonly ILogger<ClubScraperService> _logger;
    private readonly IScrapeLedgerService _scrapeLedgerService;
    private readonly TimeProvider _timeProvider;

    public ClubScraperService(
        IEnumerable<IClubScraper> clubScrapers,
        ILogger<ClubScraperService> logger,
        IScrapeLedgerService scrapeLedgerService,
        TimeProvider timeProvider,
        IOptions<BackgroundJobOptions> options)
    {
        _clubScrapers = clubScrapers;
        _logger = logger;
        _scrapeLedgerService = scrapeLedgerService;
        _timeProvider = timeProvider;

        _timer = new CronTimer(options.Value.ClubScrapeCron, _timeProvider);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Daily club scraping service started at: {Time}", _timeProvider.GetUtcNow());
        _logger.LogInformation("Next club scraping scheduled at: {Time}", _timer.GetNextOccurrence());

        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Daily club scraping started at: {Time}", _timeProvider.GetUtcNow());

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

            foreach (var scraper in _clubScrapers)
            {
                var scrapeInterval = _timeProvider.GetUtcNow().AddMonths(-1).UtcDateTime;
                if (await HasAlreadyScraped(scraper, scrapeInterval))
                {
                    continue;
                }

                await _scrapeLedgerService.Add(scraper.League.Name, scraper.League.CountryId, ScrapeTypes.ClubInfo);
                await Scrape(browser, scraper);
            }

            await browser.CloseAsync();

            _logger.LogInformation("Daily club scraping finished at: {Time}", _timeProvider.GetUtcNow());
            _logger.LogInformation("Next club scraping scheduled at: {Time}", _timer.GetNextOccurrence());
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