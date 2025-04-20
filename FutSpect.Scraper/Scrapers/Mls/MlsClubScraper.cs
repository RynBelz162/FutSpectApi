using FutSpect.Scraper.Extensions;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Extensions;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Mls;

public partial class MlsClubScraper : IClubScraper
{
    const string LeagueSiteUrl = "https://mlssoccer.com";
    public string LeagueName => "Major League Soccer";
    public int CountryId => Countries.USA;

    private readonly ILeagueService _leagueService;

    public MlsClubScraper(ILeagueService leagueService)
    {
        _leagueService = leagueService;
    }

    public async Task<ClubScrapeInfo[]> ScrapeClubs(IBrowserContext browserContext)
    {
        var leagueId = await _leagueService.GetLeagueId(LeagueName, CountryId);

        var clubs = await browserContext.OpenPageAndExecute<ClubScrapeInfo[]>($"{LeagueSiteUrl}/clubs", async (page) =>
        {
            var clubs = await page.Locator(".mls-o-clubs-hub-clubs-list__club").AllAsync();

            var clubInfoTasks = clubs.Select(x => ScrapeClub(x, leagueId));

            var results = await Task.WhenAll(clubInfoTasks);
            return [.. results.WhereNotNull()];
        });

        return clubs;
    }

    private static async Task<ClubScrapeInfo?> ScrapeClub(ILocator locator, int leagueId)
    {
        var clubLogo = locator.Locator(".mls-o-clubs-hub-clubs-list__club-logo");

        var imageElement = clubLogo.Locator("picture").Locator("img");
        var imageSrc = await imageElement.GetAttributeAsync("src");
        var name = await imageElement.GetAttributeAsync("alt");

        if (string.IsNullOrWhiteSpace(imageSrc) || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);

        var links = locator.Locator(".mls-o-clubs-hub-clubs-list__club-info")
            .Locator(".mls-o-clubs-hub-clubs-list__club-links");

        var detailsHref = await links.GetByText("Details").GetAttributeAsync("href");
        var scheduleHref = await links.GetByText("Schedule").GetAttributeAsync("href");

        return new ClubScrapeInfo
        {
            Name = name,
            LeagueId = leagueId,
            Image = new()
            {
                ImageSrcUrl = imageSrc,
                ImageBytes = imageBytes,
                ImageExtension = imageExtension,
            },
            RosterUrl = $"{LeagueSiteUrl}{detailsHref}roster",
            ScheduleUrl = $"{LeagueSiteUrl}{scheduleHref}"
        };
    }
}