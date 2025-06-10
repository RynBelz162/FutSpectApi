using FutSpect.Scraper.Constants;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Mls;

public class MlsPlayScraper : IPlayerScraper
{
    const string PlayerSiteUrl = "https://www.mlssoccer.com/players/*/";

    private readonly IPlayerInfoParseService _playerInfoParseService;
    private readonly ISanitizeService _sanitizeService;

    public MlsPlayScraper(IPlayerInfoParseService playerInfoParseService, ISanitizeService sanitizeService)
    {
        _playerInfoParseService = playerInfoParseService;
        _sanitizeService = sanitizeService;
    }

    public async Task<List<PlayerScrapeInfo>> Scrape(IBrowserContext browserContext, string rosterUrl)
    {
        var page = await browserContext.NewPageAsync();

        await page.GotoAsync(rosterUrl);
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
        var number = _playerInfoParseService.GetNumber(header);

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
                (firstName, lastName) = _playerInfoParseService.GetName(value);
                continue;
            }

            if (string.Equals(property, MlsPlayerElementConstants.Position, StringComparison.OrdinalIgnoreCase))
            {
                positionId = _playerInfoParseService.GetPositionId(value);
            }

            if (string.Equals(property, MlsPlayerElementConstants.Birthplace, StringComparison.OrdinalIgnoreCase))
            {
                birthPlace = _sanitizeService.Sanitize(value);
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