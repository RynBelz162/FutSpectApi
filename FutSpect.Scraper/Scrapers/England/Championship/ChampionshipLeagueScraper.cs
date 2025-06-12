using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Image;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.England.Championship;

public class ChampionshipLeagueScraper : ILeagueScraper
{
    public string LeagueName => "EFL Championship";
    public int CountryId => Countries.England;
    private const string LeagueUrl = "https://www.efl.com/competitions/efl-championship/";

    private readonly IImageService _imageService;

    public ChampionshipLeagueScraper(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext)
    {
        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(LeagueUrl);

        var imageElement = page.Locator(".footer-logo");
        var imageSrc = await imageElement.GetAttributeAsync("src");

        var imageResult = await _imageService.DownloadImageAsync(LeagueUrl, imageSrc);
        if (!imageResult.IsSuccess)
        {
            return null;
        }

        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "EFLC",
            HasProRel = true,
            PyramidLevel = 2,
            Website = LeagueUrl,
            CountryId = CountryId,
            Image = imageResult.Value
        };

        await page.CloseAsync();
        return leagueScrapeInfo;
    }
}