using Microsoft.Playwright;

namespace FutSpect.Scraper.Extensions;

public static class BrowserExtensions
{
    public static async Task<T> OpenPageAndExecute<T>(this IBrowser browser, string pageUrl, Func<IPage, Task<T>> task)
    {
       var page = await browser.NewPageAsync();

       await page.GotoAsync(pageUrl);

       var result = await task(page);

       await page.CloseAsync();

       return result;
    }
}