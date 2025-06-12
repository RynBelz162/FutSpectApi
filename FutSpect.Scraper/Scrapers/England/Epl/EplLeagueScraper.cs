using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Image;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.England.Epl;

public class EplLeagueScraper : ILeagueScraper
{
    public string LeagueName => "English Premier League";
    public int CountryId => Countries.England;
    private const string LeagueUrl = "https://www.premierleague.com/";

    private readonly IImageService _imageService;
    public EplLeagueScraper(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext)
    {
        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(LeagueUrl);

        var imageElement = page.Locator(".pl-header-logo");
        var imageSrc = await imageElement.GetAttributeAsync("src");

        var imageResult = await _imageService.DownloadImageAsync(LeagueUrl, imageSrc);

        if (!imageResult.IsSuccess)
        {
            return null;
        }

        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "EPL",
            HasProRel = true,
            PyramidLevel = 1,
            Website = LeagueUrl,
            CountryId = CountryId,
            Image = imageResult.Value
        };

        await page.CloseAsync();
        return leagueScrapeInfo;
    }
}