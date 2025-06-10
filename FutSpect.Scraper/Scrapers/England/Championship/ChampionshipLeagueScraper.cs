using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services;
using FutSpect.Shared.Constants;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Scrapers.England.Championship;

public class ChampionshipLeagueScraper : ILeagueScraper
{
    public string LeagueName => "EFL Championship";
    public int CountryId => Countries.England;
    private const string LeagueUrl = "https://www.efl.com/competitions/efl-championship/";

    private static string GetBaseUrl()
    {
        Uri uri = new(LeagueUrl);
        return $"{uri.Scheme}://{uri.Host}";
    }

    public async Task<LeagueScrapeInfo?> Scrape(IBrowserContext browserContext)
    {
        var page = await browserContext.NewPageAsync();
        await page.GotoAsync(LeagueUrl);

        var imageElement = page.Locator(".footer-logo");
        var imageSrc = await imageElement.GetAttributeAsync("src");

        if (imageSrc is null)
        {
            return null;
        }

        if (imageSrc.StartsWith('/'))
        {
            // Handle protocol-relative URLs
            imageSrc = GetBaseUrl() + imageSrc;
        }

        var (imageBytes, imageExtension) = await ImageDownloaderService.DownloadImageAsync(imageSrc);

        var leagueScrapeInfo = new LeagueScrapeInfo
        {
            Name = LeagueName,
            Abbreviation = "EFLC",
            HasProRel = true,
            PyramidLevel = 2,
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