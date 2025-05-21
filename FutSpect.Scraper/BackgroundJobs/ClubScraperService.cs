using FutSpect.DAL.Constants;
using FutSpect.Scraper.Options;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Clubs;
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
    private readonly IClubService _clubService;

    public ClubScraperService(
        IEnumerable<IClubScraper> clubScrapers,
        ILogger<ClubScraperService> logger,
        IScrapeLedgerService scrapeLedgerService,
        TimeProvider timeProvider,
        IOptions<BackgroundJobOptions> options,
        IClubService clubService)
    {
        _timer = new CronTimer(options.Value.ClubScrapeCron, timeProvider);

        _clubScrapers = clubScrapers;
        _logger = logger;
        _scrapeLedgerService = scrapeLedgerService;
        _timeProvider = timeProvider;
        _clubService = clubService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Club scraping service started at: {Time}", _timeProvider.GetUtcNow());
        _logger.LogInformation("Next club scraping scheduled at: {Time}", _timer.GetNextOccurrence());

        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Club scraping started at: {Time}", _timeProvider.GetUtcNow());

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

            _logger.LogInformation("Club scraping finished at: {Time}", _timeProvider.GetUtcNow());
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
            var clubInfo = await scraper.Scrape(context);
            await _clubService.Add(clubInfo);
            
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