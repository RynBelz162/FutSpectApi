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

        serviceCollection.AddSingleton<ILeagueRepository, LeagueRepository>();
        serviceCollection.AddSingleton<IScrapeLedgerRepository, ScrapeLedgerRepository>();

        return serviceCollection;
    }
}