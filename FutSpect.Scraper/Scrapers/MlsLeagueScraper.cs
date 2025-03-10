using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Leagues.Mls;
using FutSpect.Shared.Extensions;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public class MlsLeagueScraper(IMlsLeagueService mlsLeagueService) : ILeagueScraper
{
    const string LeagueSiteUrl = "https://mlssoccer.com";

    public async Task Scrape(IBrowser browser)
    {   
        var leagueId = await mlsLeagueService.GetLeagueId();

        var page = await browser.NewPageAsync();

        await page.GotoAsync($"{LeagueSiteUrl}/clubs");
        var clubs = await ScrapeClubs(page);
    }

    private static async Task<ClubScrapeInfo[]> ScrapeClubs(IPage page)
    {
        var clubs = await page.Locator(".mls-o-clubs-hub-clubs-list__club").AllAsync();

        var clubInfoTasks = clubs.Select(ScrapeClub);

        var results = await Task.WhenAll(clubInfoTasks);

        return [.. results.WhereNotNull()];
    }

    private static async Task<ClubScrapeInfo?> ScrapeClub(ILocator locator)
    {
        var clubLogo = locator.Locator(".mls-o-clubs-hub-clubs-list__club-logo");

        var imageElement = clubLogo.Locator("picture").Locator("img");
        var image = await imageElement.GetAttributeAsync("src");
        var name = await imageElement.GetAttributeAsync("alt");

        if (string.IsNullOrWhiteSpace(image) || string.IsNullOrWhiteSpace(name))
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
            ImageSrcUrl = image,
            RosterUrl = $"{LeagueSiteUrl}{detailsHref}roster",
            ScheduleUrl = $"{LeagueSiteUrl}{scheduleHref}"
        };
    }
}