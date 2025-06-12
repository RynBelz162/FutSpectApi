using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Image;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Usl.Championship;

public class UslChampionshipLeagueScraper : ILeagueScraper
{
    public string LeagueName => "USL Championship";
    public int CountryId => Countries.USA;
    private const string LeagueUrl = "https://www.uslchampionship.com/";

    private readonly IImageService _imageService;
    
    public UslChampionshipLeagueScraper(IImageService imageService)
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
            Abbreviation = "USLC",
            HasProRel = false,
            PyramidLevel = 2,
            Website = LeagueUrl,
            CountryId = CountryId,
            Image = imageResult.Value
        };

        await page.CloseAsync();
        return leagueScrapeInfo;
    }
}
