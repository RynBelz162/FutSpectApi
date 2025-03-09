using FutSpect.DAL.Entities.Clubs;
using FutSpect.DAL.Entities.Countries;
using FutSpect.DAL.Entities.Leagues;
using FutSpect.DAL.Entities.Scraping;
using FutSpect.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL;

public class FutSpectContext : DbContext
{
    public FutSpectContext()
    {
    }

    public FutSpectContext(DbContextOptions<FutSpectContext> options) : base(options)
    {
    }

    public DbSet<ClubEntity> Clubs { get; set; }

    public DbSet<CountryEntity> Countries { get; set; }

    public DbSet<ClubLogoEntity> ClubLogos { get; set; }

    public DbSet<LeagueEntity> Leagues { get; set; }

    public DbSet<ScrapeLedgerEntity> ScrapeLedgers { get; set; }

    public DbSet<ScrapeTypeEntity> ScrapeTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured)
        {
            return;
        }

        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;");

        var lookupEntities = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(ILookupTable).IsAssignableFrom(type) && type.IsClass);

        options.UseSeeding((ctx, _) =>
        {
            foreach (var type in lookupEntities)
            {
                var entity = ctx.Model.FindEntityType(type);
                if (entity is null)
                {
                    continue;
                }

                ((ILookupTable)entity.ClrType).SeedData(ctx);
            }
        });

        options.UseAsyncSeeding(async (ctx, _, cancellationToken) =>
        {
            foreach (var type in lookupEntities)
            {
                var entity = ctx.Model.FindEntityType(type);
                if (entity is null)
                {
                    continue;
                }

                await ((ILookupTable)entity.ClrType).SeedDataAsync(ctx, cancellationToken);
            }
        });
    }
}
