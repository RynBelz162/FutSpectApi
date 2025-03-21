using System.Text.RegularExpressions;
using FutSpect.Scraper.Constants;
using FutSpect.Scraper.Extensions;
using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Leagues.Mls;
using FutSpect.Scraper.Services.Scraping;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Extensions;
using FutSpect.Shared.Models.Players;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers;

public partial class MlsLeagueScraper
(
    IMlsLeagueService mlsLeagueService,
    IPlayerService playerService
) : ILeagueScraper
{
    const string LeagueSiteUrl = "https://mlssoccer.com";

    public async Task Scrape(IBrowser browser)
    {   
        var leagueId = await mlsLeagueService.GetLeagueId();

        var clubs = await browser.OpenPageAndExecute($"{LeagueSiteUrl}/clubs", ScrapeClubs);

        foreach (var club in clubs)
        {
            await ScrapePlayers(browser, club);
        }
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

    private async Task<List<PlayerInfo>> ScrapePlayers(IBrowser browser, ClubScrapeInfo clubScrapeInfo)
    {
        var page = await browser.NewPageAsync();

        await page.GotoAsync(clubScrapeInfo.RosterUrl);
        await page.WaitForSelectorAsync(".mls-c-active-roster__table");

        var rows = await page
            .Locator(".mls-c-active-roster__table")
            .Locator(".mls-o-table__body")
            .GetByRole(AriaRole.Row)
            .AllAsync();

        var players = new List<PlayerInfo>();

        foreach (var row in rows)
        {
            var cells = await row.GetByRole(AriaRole.Cell).AllAsync();
            var firstCell = cells.FirstOrDefault();

            if (firstCell is null)
            {
                continue;
            }

            await firstCell.Locator(".mls-o-table__href").ClickAsync();
            await page.WaitForURLAsync("https://www.mlssoccer.com/players/*/");

            short number = 0;
            var header = await page.Locator(".mls-o-masthead__text").TextContentAsync();
            if (header is not null)
            {
                var match = NumberRegex().Match(header);
                if (match.Success && short.TryParse(match.Value, out var numberRaw))
                {
                    number = numberRaw;
                }
            }

            string? firstName = null;
            string lastName = string.Empty;
            int positionId = 0;

            var infoElements = await page.Locator(".mls-l-module--player-status-details__info").AllAsync();
            foreach (var infoElement in infoElements)
            {
                var property = await infoElement.Locator("h3").TextContentAsync();

                if (string.Equals(property, MlsPlayerElementConstants.Name))
                {
                    var value = await infoElement.Locator("span").TextContentAsync() ?? string.Empty;
                    (firstName, lastName) = playerService.GetName(value);
                    continue;
                }

                if (string.Equals(property, MlsPlayerElementConstants.Position))
                {
                    var value = await infoElement.Locator("span").TextContentAsync() ?? string.Empty;
                    positionId = playerService.GetPositionId(value);
                }
            }

            players.Add(new PlayerInfo(firstName, lastName, positionId, number));
        }

        await page.CloseAsync();
        return players;
    }

    [GeneratedRegex("#[\\d]")]
    private static partial Regex NumberRegex();
}