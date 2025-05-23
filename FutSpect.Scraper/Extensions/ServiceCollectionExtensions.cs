using System.Reflection;
using FutSpect.Scraper.BackgroundJobs;
using FutSpect.Scraper.Options;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Clubs;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FutSpect.Scraper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPlayerInfoParseService, PlayerInfoParseService>();
        serviceCollection.AddSingleton<ISanitizeService, SanitizeService>();
        serviceCollection.AddSingleton<IScrapeLedgerService, ScrapeLedgerService>();
        serviceCollection.AddSingleton<ILeagueService, LeagueService>();
        serviceCollection.AddSingleton<IClubService, ClubService>();

        return serviceCollection;
    }

    public static IServiceCollection AddScrapers(this IServiceCollection serviceCollection)
    {
        var scraperTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IClubScraper).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var scraperType in scraperTypes)
        {
            serviceCollection.AddSingleton(typeof(IClubScraper), scraperType);
        }

        return serviceCollection;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHostedService<ClubScraperService>();
        return serviceCollection;
    }

    public static IServiceCollection AddConfigOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<BackgroundJobOptions>(
            configuration.GetSection("BackgroundJobs"));

        return serviceCollection;
    }
}