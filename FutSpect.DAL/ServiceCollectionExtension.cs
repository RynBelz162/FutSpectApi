using FutSpect.DAL.Repositories.Clubs;
using FutSpect.DAL.Repositories.Leagues;
using FutSpect.DAL.Repositories.Scraping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FutSpect.DAL;

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

        return serviceCollection;
    }
}