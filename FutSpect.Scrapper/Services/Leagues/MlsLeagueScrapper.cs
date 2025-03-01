using FutSpect.Scrapper.Interfaces;
using FutSpect.Shared.Extensions;
using FutSpect.Shared.Models;
using Microsoft.Playwright;

namespace FutSpect.Scrapper.Services.Leagues;

public class MlsLeagueScrapper : ILeagueScrapper
{
    public async Task Scrap(IBrowser browser)
    {
        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://mlssoccer.com/clubs");

        var clubs = await ScrapClubs(page);
    }

    private static async Task<ClubInfo[]> ScrapClubs(IPage page)
    {
        await page.GotoAsync("https://mlssoccer.com/clubs");
        var clubLogos = await page.Locator(".mls-o-clubs-hub-clubs-list__club-logo").AllAsync();

        var clubInfoTasks = clubLogos.Select(GetClubInfo);

        var results = await Task.WhenAll(clubInfoTasks);

        return [.. results.WhereNotNull()];
    }

    private static async Task<ClubInfo?> GetClubInfo(ILocator locator)
    {
        var imageElement = locator.Locator("picture").Locator("img");
        var image = await imageElement.GetAttributeAsync("src");
        var name = await imageElement.GetAttributeAsync("alt");

        if (string.IsNullOrWhiteSpace(image) || string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var (Bytes, Extension) = await ImageDownloaderService.DownloadImageAsync(image);

        var clubLogo = new ClubLogo(image, Bytes, Extension);
        return new ClubInfo(name, clubLogo);
    }
}