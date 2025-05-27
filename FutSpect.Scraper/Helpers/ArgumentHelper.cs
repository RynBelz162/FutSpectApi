using FutSpect.Scraper.Models;

namespace FutSpect.Scraper.Helpers;

public static class ArgumentHelper
{
    public static ScraperArgs ParseArguments(string[] args, string name)
    {
        bool runClubNow = false;
        bool runLeagueNow = false;

        var match = args.FirstOrDefault(a => a.StartsWith(name, StringComparison.OrdinalIgnoreCase));
        if (match != null && match.Length > name.Length)
        {
            var listPart = match[name.Length..];
            var items = listPart.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var item in items)
            {
                if (item.Equals("class", StringComparison.OrdinalIgnoreCase))
                {
                    runClubNow = true;
                }

                if (item.Equals("league", StringComparison.OrdinalIgnoreCase))
                {
                    runLeagueNow = true;
                }
            }
        }

        return new ScraperArgs(runClubNow, runLeagueNow);
    }
}
