using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Image;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.Usa.Mls;

public class MlsLeagueScraper : ILeagueScraper
{
    public string LeagueName => "Major League Soccer";
    public int CountryId => Countries.USA;
    private const string LeagueUrl = "https://www.mlssoccer.com/";

    private readonly IImageService _imageService;
    
    public MlsLeagueScraper(IImageService imageService)
    {
        _imageService = imageService;
    }

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

        var imageResult = await _imageService.DownloadImageAsync(LeagueUrl, imageSrc);
        if (!imageResult.IsSuccess)
        {
            return null;
        }

        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "MLS",
            HasProRel = false,
            PyramidLevel = 1,
            Website = LeagueUrl,
            CountryId = CountryId,
            Image = imageResult.Value
        };

        await page.CloseAsync();
        return leagueScrapeInfo;
    }
}