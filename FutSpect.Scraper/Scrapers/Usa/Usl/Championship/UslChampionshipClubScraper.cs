using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Scraper.Services.Scraping;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Models.Leagues;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Usl.Championship;

public partial class UslChampionshipClubScraper : IClubScraper
{
    const string LeagueSiteUrl = "https://www.uslchampionship.com";
    const string TeamsUrl = $"{LeagueSiteUrl}/league-teams";

    private readonly ILeagueService _leagueService;
    private readonly ISanitizeService _sanitizeService;

    public UslChampionshipClubScraper(ILeagueService leagueService, ISanitizeService sanitizeService)
    {
        _leagueService = leagueService;
        _sanitizeService = sanitizeService;
    }

    public League League => new()
    {
        Name = "USL Championship",
        Abbreviation = "USLC",
        CountryId = Countries.USA,
        HasProRel = false,
        PyramidLevel = 2
    };

    public async Task<ClubScrapeInfo[]> Scrape(IBrowserContext browserContext)
    {
        var leagueId = await _leagueService.GetId(League);
        var page = await browserContext.NewPageAsync();

        await page.GotoAsync(TeamsUrl);
        await page.WaitForSelectorAsync(".more");

        var clubLocators = await page.Locator(".more").AllAsync();

        List<ClubScrapeInfo> results = [];
        foreach (var locator in clubLocators)
        {
            var club = await ScrapeClub(browserContext, locator, leagueId);
            if (club is null)
            {
                continue;
            }

            results.Add(club);
        }

        await page.CloseAsync();
        return [.. results];
    }

    private async Task<ClubScrapeInfo?> ScrapeClub(IBrowserContext browserContext, ILocator locator, int leagueId)
    {
        var rosterUrl = await locator
            .GetByRole(AriaRole.Paragraph)
            .Nth(2)
            .GetByRole(AriaRole.Link, new() { Name = "Roster" })
            .GetAttributeAsync("href");

        var scheduleUrl = await locator
            .GetByRole(AriaRole.Paragraph)
            .Nth(0)
            .GetByRole(AriaRole.Link, new() { Name = "Schedule" })
            .GetAttributeAsync("href");

        if (string.IsNullOrEmpty(rosterUrl) || string.IsNullOrEmpty(scheduleUrl))
        {
            return null;
        }

        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(scheduleUrl);
        await page.WaitForSelectorAsync(".clubLogo");

        var imageSrc = await page
            .Locator(".clubLogo")
            .GetByRole(AriaRole.Img)
            .GetAttributeAsync("src");

        var name = await page.Locator(".teamInfo")
            .GetByRole(AriaRole.Heading)
            .InnerTextAsync();

        if (string.IsNullOrWhiteSpace(imageSrc) || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);

        await page.CloseAsync();

        return new ClubScrapeInfo
        {
            Name = _sanitizeService.ToTitleCase(name),
            LeagueId = leagueId,
            Image = new()
            {
                ImageSrcUrl = imageSrc,
                ImageBytes = imageBytes,
                ImageExtension = imageExtension,
            },
            RosterUrl = $"{LeagueSiteUrl}{rosterUrl}",
            ScheduleUrl = $"{LeagueSiteUrl}{scheduleUrl}"
        };
    }
}