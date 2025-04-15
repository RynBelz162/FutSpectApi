using FutSpect.Scraper.Extensions;
using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Scraper.Services.Leagues.Usl;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usl;

public partial class UslLeagueScraper
(
    IUslLeagueService uslLeagueService,
    IPlayerInfoParseService playerInfoParseService,
    ISanitizeService sanitizeService
) : ILeagueScraper
{
    const string LeagueSiteUrl = "https://www.uslchampionship.com";
    const string TeamsUrl = $"{LeagueSiteUrl}/league-teams";

    public async Task Scrape(IBrowserContext browserContext)
    {   
        var leagueId = await uslLeagueService.GetLeagueId();

        var clubs = await browserContext.OpenPageAndExecute(TeamsUrl, ScrapeClubs);

        // foreach (var club in clubs)
        // {
        //     await ScrapePlayers(browserContext, club);
        // }
    }

    private async Task<ClubScrapeInfo[]> ScrapeClubs(IBrowserContext browserContext, IPage page)
    {
        await page.WaitForSelectorAsync(".more");
        
        var clubLocators = await page.Locator(".more").AllAsync();

        List<ClubScrapeInfo> results = [];
        foreach (var locator in clubLocators)
        {
            var club = await ScrapeClub(browserContext, locator);
            if (club is null)
            {
                continue;
            }

            results.Add(club);
        }

        return [.. results];
    }

    private async Task<ClubScrapeInfo?> ScrapeClub(IBrowserContext browserContext, ILocator locator)
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
            Name = sanitizeService.ToTitleCase(name),
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