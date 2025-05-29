using FutSpect.Api.Services.Leagues;

namespace FutSpect.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<ILeagueService, LeagueService>();

        return services;
    }
}