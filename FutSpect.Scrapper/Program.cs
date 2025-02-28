using FutSpect.Scrapper.Services;
using FutSpect.Shared.Models;
using Microsoft.Playwright;

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
var page = await browser.NewPageAsync();

await page.GotoAsync("https://mlssoccer.com/clubs");

var clubLogos = await page.Locator(".mls-o-clubs-hub-clubs-list__club-logo").AllAsync();

var clubInfoTasks = clubLogos.Select(GetClubInfo);

var results = await Task.WhenAll(clubInfoTasks);
var clubInfo = results
    .Where(x => x is not null)
    .ToList();

Console.ReadLine();

static async Task<ClubInfo?> GetClubInfo(ILocator locator)
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