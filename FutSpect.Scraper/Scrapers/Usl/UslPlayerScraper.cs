using FutSpect.Scraper.Models;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usl;

public class UslPlayerScraper : IPlayerScraper
{
    // TODO: USL Player Scraper not currently working
    public Task<List<PlayerScrapeInfo>> Scrape(IBrowserContext browserContext, string rosterUrl)
    {
        return Task.FromResult<List<PlayerScrapeInfo>>([]);
    }

    // private async Task<List<PlayerScrapeInfo>> ScrapePlayers(IBrowserContext browserContext, ClubScrapeInfo clubScrapeInfo)
    // {
    //     var page = await browserContext.NewPageAsync();
    //     try
    //     {
    //         await page.GotoAsync(clubScrapeInfo.RosterUrl);

    //         // Wait for dynamic content to load
    //         await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

    //         // The roster data is loaded dynamically, so we need to wait for it
    //         await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
    //         await page.WaitForSelectorAsync(".roster-player, [data-player-id], .player-card", new() 
    //         { 
    //             State = WaitForSelectorState.Visible,
    //             Timeout = 10000  // 10 second timeout
    //         });

    //         // Try to locate player cards with various possible selectors
    //         var playerCards = await page.Locator(".roster-player, [data-player-id], .player-card").AllAsync();

    //         var playerScrapeTasks = playerCards.Select(async player =>
    //         {
    //             try 
    //             {
    //                 return await ScrapePlayer(player);
    //             }
    //             catch
    //             {
    //                 return null;
    //             }
    //         });

    //         var playerInfos = await Task.WhenAll(playerScrapeTasks);
    //         return playerInfos.WhereNotNull().ToList();
    //     }
    //     catch
    //     {
    //         // Log error if needed
    //         return new List<PlayerScrapeInfo>();
    //     }
    //     finally
    //     {
    //         await page.CloseAsync();
    //     }
    // }

    // private async Task<PlayerScrapeInfo?> ScrapePlayer(ILocator playerCard)
    // {
    //     try
    //     {
    //         // Get player details with multiple possible selector combinations
    //         var nameElement = playerCard.Locator(".player-name, [data-player-name], .name").First;
    //         var numberElement = playerCard.Locator(".player-number, [data-player-number], .number, .jersey").First;
    //         var positionElement = playerCard.Locator(".player-position, [data-player-position], .position").First;
    //         var imageElement = playerCard.Locator(".player-image img, .headshot img, img").First;

    //         // Wait for all elements with timeout
    //         await Task.WhenAll(
    //             nameElement.WaitForAsync(new() { Timeout = 5000 }),
    //             numberElement.WaitForAsync(new() { Timeout = 5000 }),
    //             positionElement.WaitForAsync(new() { Timeout = 5000 }),
    //             imageElement.WaitForAsync(new() { Timeout = 5000 })
    //         );

    //         var name = await nameElement.TextContentAsync();
    //         var numberText = await numberElement.TextContentAsync();
    //         var position = await positionElement.TextContentAsync();
    //         var imageSrc = await imageElement.GetAttributeAsync("src");

    //         if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(position) || 
    //             string.IsNullOrWhiteSpace(numberText) || string.IsNullOrWhiteSpace(imageSrc))
    //         {
    //             return null;
    //         }

    //         var (firstName, lastName) = playerInfoParseService.GetName(sanitizeService.Sanitize(name));
    //         var positionId = playerInfoParseService.GetPositionId(sanitizeService.Sanitize(position));
    //         var number = playerInfoParseService.GetNumber(numberText);

    //         var (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);

    //         return new PlayerScrapeInfo
    //         {
    //             FirstName = firstName,
    //             LastName = lastName,
    //             PositionId = positionId,
    //             Number = number,
    //             Image = new()
    //             {
    //                 ImageSrcUrl = imageSrc,
    //                 ImageBytes = imageBytes,
    //                 ImageExtension = imageExtension
    //             }
    //         };
    //     }
    //     catch
    //     {
    //         return null;
    //     }
    // }
}