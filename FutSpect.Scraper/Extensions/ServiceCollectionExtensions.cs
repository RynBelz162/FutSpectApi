using FutSpect.Scraper.Interfaces;
using FutSpect.Scraper.Scrapers.Mls;
using FutSpect.Scraper.Scrapers.Usl;
using FutSpect.Scraper.Services.Leagues.Mls;
using FutSpect.Scraper.Services.Leagues.Usl;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Extensions.DependencyInjection;

namespace FutSpect.Scraper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPlayerInfoParseService, PlayerInfoParseService>();
        serviceCollection.AddSingleton<ISanitizeService, SanitizeService>();
        serviceCollection.AddSingleton<IScrapeLedgerService, ScrapeLedgerService>();

        serviceCollection.AddSingleton<IMlsLeagueService, MlsLeagueService>();
        serviceCollection.AddSingleton<IUslLeagueService, UslLeagueService>();

        return serviceCollection;
    }

    public static IServiceCollection AddScrapers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILeagueScraper, MlsLeagueScraper>();
        serviceCollection.AddSingleton<ILeagueScraper, UslLeagueScraper>();

        return serviceCollection;
    }
}