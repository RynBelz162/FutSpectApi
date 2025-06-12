using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Image;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Extensions;
using FutSpect.Shared.Models.Leagues;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Mls;

public partial class MlsClubScraper : IClubScraper
{
    const string LeagueSiteUrl = "https://mlssoccer.com";

    private readonly ILeagueService _leagueService;
    private readonly IImageService _imageService;

    public MlsClubScraper(ILeagueService leagueService, IImageService imageService)
    {
        _leagueService = leagueService;
        _imageService = imageService;
    }

    public League League => new()
    {
        Name = "Major League Soccer",
        Abbreviation = "MLS",
        CountryId = Countries.USA,
        HasProRel = false,
        PyramidLevel = 1
    };

    public async Task<ClubScrapeInfo[]> Scrape(IBrowserContext browserContext)
    {
        var leagueId = await _leagueService.GetId(League);
        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(LeagueSiteUrl);

        var clubs = await page.Locator(".mls-o-clubs-hub-clubs-list__club").AllAsync();

        var clubInfoTasks = clubs.Select(x => ScrapeClub(x, leagueId));

        var results = await Task.WhenAll(clubInfoTasks);

        await page.CloseAsync();
        return [.. results.WhereNotNull()];
    }

    private async Task<ClubScrapeInfo?> ScrapeClub(ILocator locator, int leagueId)
    {
        var clubLogo = locator.Locator(".mls-o-clubs-hub-clubs-list__club-logo");

        var imageElement = clubLogo.Locator("picture").Locator("img");
        var imageSrc = await imageElement.GetAttributeAsync("src");
        var name = await imageElement.GetAttributeAsync("alt");

        if (string.IsNullOrWhiteSpace(imageSrc) || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var imageResult = await _imageService.DownloadImageAsync(LeagueSiteUrl, imageSrc);
        if (!imageResult.IsSuccess)
        {
            return null;
        }

        var links = locator.Locator(".mls-o-clubs-hub-clubs-list__club-info")
            .Locator(".mls-o-clubs-hub-clubs-list__club-links");

        var detailsHref = await links.GetByText("Details").GetAttributeAsync("href");
        var scheduleHref = await links.GetByText("Schedule").GetAttributeAsync("href");

        return new ClubScrapeInfo
        {
            Name = name,
            LeagueId = leagueId,
            Image = imageResult.Value,
            RosterUrl = $"{LeagueSiteUrl}{detailsHref}roster",
            ScheduleUrl = $"{LeagueSiteUrl}{scheduleHref}"
        };
    }
}