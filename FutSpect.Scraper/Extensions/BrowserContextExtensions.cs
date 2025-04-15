using Microsoft.Playwright;

namespace FutSpect.Scraper.Extensions;

public static class BrowserContextExtensions
{
    public static async Task<T> OpenPageAndExecute<T>(this IBrowserContext browserContext, string pageUrl, Func<IPage, Task<T>> task)
    {
       var page = await browserContext.NewPageAsync();

       await page.GotoAsync(pageUrl);

       var result = await task(page);

       await page.CloseAsync();

       return result;
    }

    public static async Task<T> OpenPageAndExecute<T>(this IBrowserContext browserContext, string pageUrl, Func<IBrowserContext, IPage, Task<T>> task)
    {
       var page = await browserContext.NewPageAsync();

       await page.GotoAsync(pageUrl);

       var result = await task(browserContext, page);

       await page.CloseAsync();

       return result;
    }
}