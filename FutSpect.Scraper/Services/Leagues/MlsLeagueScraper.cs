using FutSpect.DAL.Repositories.Leagues;
using FutSpect.Scraper.Interfaces;
using FutSpect.Shared.Constants;
using FutSpect.Shared.Extensions;
using FutSpect.Shared.Models;
using FutSpect.Shared.Models.Leagues;
using Microsoft.Playwright;

namespace FutSpect.Scraper.Services.Leagues;

public class MlsLeagueScraper(ILeagueRepository leagueRepository) : ILeagueScraper
{
    public async Task Scrape(IBrowser browser)
    {   
        var leagueId = await GetLeagueId();

        var page = await browser.NewPageAsync();

        await page.GotoAsync("https://mlssoccer.com/clubs");

        var clubs = await ScrapeClubs(page);
    }

    private async Task<int> GetLeagueId()
    {
        var mlsLeague = await leagueRepository.Get("Major League Soccer");
        if (mlsLeague is not null)
        {
            return mlsLeague.Id;
        }

        var league = new League
        {
            Name = "Major League Soccer",
            CountryId = Countries.USA,
        };

        return await leagueRepository.Save(league);
    }

    private static async Task<ClubInfo[]> ScrapeClubs(IPage page)
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