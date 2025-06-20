using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Image;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Usl.LeagueTwo;

public class UslLeagueTwoLeagueScraper : ILeagueScraper
{
    public string LeagueName => "USL League Two";
    public int CountryId => Countries.USA;
    private const string LeagueUrl = "https://www.uslleaguetwo.com/";
    
    private readonly IImageService _imageService;

    public UslLeagueTwoLeagueScraper(IImageService imageService)
    {
        _imageService = imageService;
    }

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

        var imageResult = await _imageService.DownloadImageAsync(LeagueUrl, imageSrc);
        if (!imageResult.IsSuccess)
        {
            return null;
        }

        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "USL2",
            HasProRel = false,
            PyramidLevel = 4,
            Website = LeagueUrl,
            CountryId = CountryId,
            Image = imageResult.Value
        };

        await page.CloseAsync();
        return leagueScrapeInfo;
    }
}
