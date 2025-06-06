using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usl.Championship;

public class UslLeagueScraper : ILeagueScraper
{
    private const string LeagueUrl = "https://www.uslchampionship.com/";

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
            Name = "USL Championship",
            Abbreviation = "USLC",
            HasProRel = false,
            PyramidLevel = 2,
            Website = LeagueUrl,
            CountryId = Countries.USA,
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
