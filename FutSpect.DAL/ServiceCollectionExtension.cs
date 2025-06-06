using FutSpect.Dal.Repositories.Clubs;
using FutSpect.Dal.Repositories.Images;
using FutSpect.Dal.Repositories.Leagues;
using FutSpect.Dal.Repositories.Scraping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FutSpect.Dal;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<FutSpectContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });

        serviceCollection.AddScoped<ILeagueRepository, LeagueRepository>();
        serviceCollection.AddScoped<IClubRepository, ClubRepository>();
        serviceCollection.AddScoped<IScrapeLedgerRepository, ScrapeLedgerRepository>();
        serviceCollection.AddScoped<IImageRepository, ImageRepository>();

        return serviceCollection;
    }
}