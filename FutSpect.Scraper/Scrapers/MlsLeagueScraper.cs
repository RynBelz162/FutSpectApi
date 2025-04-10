using System.Text.RegularExpressions;
using FutSpect.Scraper.Constants;
using FutSpect.Scraper.Extensions;
using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Scraper.Services.Leagues.Mls;
using FutSpect.Scraper.Services.Scraping;
using FutSpect.Shared.Extensions;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public partial class MlsLeagueScraper
(
    IMlsLeagueService mlsLeagueService,
    IPlayerInfoParseService playerInfoParseService
) : ILeagueScraper
{
    const string LeagueSiteUrl = "https://mlssoccer.com";
    const string PlayerSiteUrl = "https://www.mlssoccer.com/players/*/";

    public async Task Scrape(IBrowser browser)
    {   
        var leagueId = await mlsLeagueService.GetLeagueId();

        var clubs = await browser.OpenPageAndExecute($"{LeagueSiteUrl}/clubs", ScrapeClubs);

        foreach (var club in clubs)
        {
            await ScrapePlayers(browser, club);
        }

        Console.WriteLine("Test");
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

    private async Task<List<PlayerScrapeInfo>> ScrapePlayers(IBrowser browser, ClubScrapeInfo clubScrapeInfo)
    {
        var page = await browser.NewPageAsync();

        await page.GotoAsync(clubScrapeInfo.RosterUrl);
        await page.WaitForSelectorAsync(".mls-c-active-roster__table");

        var rows = await page
            .Locator(".mls-c-active-roster__table")
            .Locator(".mls-o-table__body")
            .GetByRole(AriaRole.Row)
            .AllAsync();

        var players = new List<PlayerScrapeInfo>();
        foreach (var row in rows)
        {
            var player = await ScrapePlayer(row, page);
            if (player is null)
            {
                continue;
            }

            players.Add(player);
        }

        await page.CloseAsync();
        return players;
    }

    private async Task<PlayerScrapeInfo?> ScrapePlayer(ILocator playerLocator, IPage page)
    {
        var cells = await playerLocator.GetByRole(AriaRole.Cell).AllAsync();
        var firstCell = cells[0];

        if (firstCell is null)
        {
            return null;
        }

        await firstCell.Locator(".mls-o-table__href").ClickAsync();
        await page.WaitForURLAsync(PlayerSiteUrl);

        var header = await page.Locator(".mls-o-masthead__text").TextContentAsync();
        var number = playerInfoParseService.GetNumber(header);

        var imageElement = page.Locator(".mls-o-masthead__branded-image > picture > img");
        var imageSrc = await imageElement.GetAttributeAsync("src");

        string imageExtension = string.Empty;
        byte[] imageBytes = [];

        if (imageSrc is not null)
        {
            (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);
        }

        string? firstName = null;
        string? birthPlace = null;
        string lastName = string.Empty;
        int positionId = 0;

        var infoElements = await page.Locator(".mls-l-module--player-status-details__info").AllAsync();
        foreach (var infoElement in infoElements)
        {
            var property = await infoElement.Locator("h3").TextContentAsync();
            var value = await infoElement.Locator("span").TextContentAsync();

            if (string.Equals(property, MlsPlayerElementConstants.Name, StringComparison.OrdinalIgnoreCase))
            {
                (firstName, lastName) = playerInfoParseService.GetName(value);
                continue;
            }

            if (string.Equals(property, MlsPlayerElementConstants.Position, StringComparison.OrdinalIgnoreCase))
            {
                positionId = playerInfoParseService.GetPositionId(value);
            }

            if (string.Equals(property, MlsPlayerElementConstants.Birthplace, StringComparison.OrdinalIgnoreCase))
            {
                birthPlace = value?.Replace("\\n", string.Empty);
            }
        }

        return new PlayerScrapeInfo
        {
            FirstName = firstName, 
            LastName = lastName,
            PositionId = positionId,
            Number = number,
            Birthplace = birthPlace,
            Image = new()
            {
                ImageBytes = imageBytes,
                ImageExtension = imageExtension,
                ImageSrcUrl = imageSrc ?? string.Empty
            }
        };
    }
}