using FutSpect.Dal.Constants;
using FutSpect.Scraper.Options;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Scraper.Services.Scraping;
using FutSpect.Scraper.Utility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace FutSpect.Scraper.BackgroundJobs;

public class LeagueScraperService : BackgroundService
{
    private readonly CronTimer _timer;
    private readonly IEnumerable<ILeagueScraper> _leagueScrapers;
    private readonly ILogger<LeagueScraperService> _logger;
    private readonly IScrapeLedgerService _scrapeLedgerService;
    private readonly TimeProvider _timeProvider;
    private readonly ILeagueService _leagueService;
    private readonly IOptions<BackgroundJobOptions> _options;

    public LeagueScraperService(
        IEnumerable<ILeagueScraper> leagueScrapers,
        ILogger<LeagueScraperService> logger,
        IScrapeLedgerService scrapeLedgerService,
        TimeProvider timeProvider,
        IOptions<BackgroundJobOptions> options,
        ILeagueService leagueService)
    {
        _timer = new CronTimer(options.Value.LeagueScrapeCron, timeProvider);

        _options = options;
        _leagueScrapers = leagueScrapers;
        _logger = logger;
        _scrapeLedgerService = scrapeLedgerService;
        _timeProvider = timeProvider;
        _leagueService = leagueService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("League scraping service started at: {Time}", _timeProvider.GetUtcNow());
        _logger.LogInformation("Next league scraping scheduled at: {Time}", _timer.GetNextOccurrence());

        if (_options.Value.ScraperArgs.RunLeagueNow)
        {
            _logger.LogInformation("League scrape timer bypass provided, running immediately.");

            await Scrape();

            _logger.LogInformation("League scrape timer bypass completed.");
            return;
        }

        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("League scraping started at: {Time}", _timeProvider.GetUtcNow());

            await Scrape();

            _logger.LogInformation("League scraping finished at: {Time}", _timeProvider.GetUtcNow());
            _logger.LogInformation("Next club scraping scheduled at: {Time}", _timer.GetNextOccurrence());
        }
    }

    private async Task Scrape()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });

        foreach (var scraper in _leagueScrapers)
        {
            var context = await browser.NewContextAsync(new()
            {
                UserAgent = Constants.UserAgents.GetRandom()
            });

            try
            {
                if (await HasBeenScrapedRecently(scraper.LeagueName, scraper.CountryId))
                {
                    _logger.LogInformation("League {LeagueName} has been scraped recently, skipping.", scraper.LeagueName);
                    continue;
                }   

                var league = await scraper.Scrape(context);
                if (league is null)
                {
                    _logger.LogWarning("League scraping failed for {LeagueName}", typeof(ILeagueScraper).Name);
                    continue;
                }

                await _leagueService.Upsert(league);
                await _scrapeLedgerService.Add(league.Name, league.CountryId, ScrapeTypes.LeagueInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "League scraping failed for {LeagueName}", typeof(ILeagueScraper).Name);
                continue;
            }
            finally
            {
                await context.CloseAsync();
            }
        }

        await browser.CloseAsync();
    }


    private async Task<bool> HasBeenScrapedRecently(string leagueName, int countryId)
    {
        var scrapeInterval = _timeProvider.GetUtcNow().AddMinutes(-5).UtcDateTime;
        return await _scrapeLedgerService.Any(leagueName, countryId, ScrapeTypes.LeagueInfo, scrapeInterval);
    }
}