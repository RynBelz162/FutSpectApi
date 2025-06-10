using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Mls;

public class MlsLeagueScraper : ILeagueScraper
{
    public string LeagueName => "Major League Soccer";
    public int CountryId => Countries.USA;
    private const string LeagueUrl = "https://www.mlssoccer.com/";

    public async Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext)
    {
        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(LeagueUrl);

        var imageElement = page.Locator(".mls-c-header__club-logo").Locator("img");
        var imageSrc = await imageElement.GetAttributeAsync("src");

        if (imageSrc is null)
        {
            return null;
        }

        var (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);


        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "MLS",
            HasProRel = false,
            PyramidLevel = 1,
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