using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Usl.LeagueOne;

public class UslLeagueOneLeagueScraper : ILeagueScraper
{
    public string LeagueName => "USL League One";
    public int CountryId => Countries.USA;
    private const string LeagueUrl = "https://www.uslleagueone.com/";

    public async Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext)
    {
        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(LeagueUrl);

        var imageElement = page
            .Locator("#megaFooter")
            .Locator(".pageEl")
            .Locator(".heroPhotoElement")
            .Locator("img");

        var imageSrc = await imageElement.GetAttributeAsync("src");

        if (imageSrc is null)
        {
            return null;
        }

        var (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);

        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "USL1",
            HasProRel = false,
            PyramidLevel = 3,
            Website = LeagueUrl,
            CountryId = CountryId,
            Image = new()
            {
                ImageBytes = imageBytes,
                ImageExtension = imageExtension,
                ImageSrcUrl = imageSrc
            }
        };

        await page.CloseAsync();
        return leagueScrapeInfo;
    }
}
