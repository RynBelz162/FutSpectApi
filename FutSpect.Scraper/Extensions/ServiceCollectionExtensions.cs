using System.Reflection;
using FutSpect.Scraper.BackgroundJobs;
using FutSpect.Scraper.Helpers;
using FutSpect.Scraper.Options;
using FutSpect.Scraper.Scrapers;
using FutSpect.Scraper.Services.Clubs;
using FutSpect.Scraper.Services.Image;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Scraper.Services.Scraping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FutSpect.Scraper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IImageService, ImageService>();
        serviceCollection.AddSingleton<IPlayerInfoParseService, PlayerInfoParseService>();
        serviceCollection.AddSingleton<ISanitizeService, SanitizeService>();
        serviceCollection.AddSingleton<IScrapeLedgerService, ScrapeLedgerService>();
        serviceCollection.AddSingleton<ILeagueService, LeagueService>();
        serviceCollection.AddSingleton<IClubService, ClubService>();

        return serviceCollection;
    }

    public static IServiceCollection AddScrapers(this IServiceCollection serviceCollection)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var clubScraperTypes = assembly
            .GetTypes()
            .Where(t => typeof(IClubScraper).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        var leagueScraperTypes = assembly
            .GetTypes()
            .Where(t => typeof(ILeagueScraper).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var scraperType in clubScraperTypes)
        {
            serviceCollection.AddSingleton(typeof(IClubScraper), scraperType);
        }

        foreach (var scraperType in leagueScraperTypes)
        {
            serviceCollection.AddSingleton(typeof(ILeagueScraper), scraperType);
        }

        return serviceCollection;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddHostedService<ClubScraperService>()
            .AddHostedService<LeagueScraperService>();

        return serviceCollection;
    }

    public static IServiceCollection AddConfigOptions(this IServiceCollection serviceCollection, IConfiguration configuration, string[] args)
    {
        var scraperArgs = ArgumentHelper.ParseArguments(args, "--run-now:");

        serviceCollection.Configure<BackgroundJobOptions>(options =>
        {
            configuration.GetSection("BackgroundJobs").Bind(options);
            options.ScraperArgs = scraperArgs;
        });

        return serviceCollection;
    }
}