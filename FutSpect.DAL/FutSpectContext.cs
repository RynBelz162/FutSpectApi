using FutSpect.DAL.Entities.Clubs;
using FutSpect.DAL.Entities.Leagues;
using FutSpect.DAL.Entities.Lookups;
using FutSpect.DAL.Entities.Players;
using FutSpect.DAL.Entities.Scraping;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.ValueGeneration;

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

    public DbSet<LeagueLogoEntity> LeagueLogos { get; set; }

    public DbSet<ScrapeLedgerEntity> ScrapeLedgers { get; set; }

    public DbSet<ScrapeTypeEntity> ScrapeTypes { get; set; }

    public DbSet<PositionEntity> Positions { get; set; }

    public DbSet<PlayerEntity> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ClubLogoEntity>()
            .Property(x => x.Id)
            .HasValueGenerator<NpgsqlSequentialGuidValueGenerator>()
            .ValueGeneratedNever();

        modelBuilder.Entity<LeagueLogoEntity>()
            .Property(x => x.Id)
            .HasValueGenerator<NpgsqlSequentialGuidValueGenerator>()
            .ValueGeneratedNever();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (options.IsConfigured)
        {
            return;
        }

        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;");

        options.UseSeeding((ctx, _) =>
        {
            CountryEntity.SeedData(ctx);
            ScrapeTypeEntity.SeedData(ctx);
            PositionEntity.SeedData(ctx);
        });

        options.UseAsyncSeeding(async (ctx, _, cancellationToken) =>
        {
            await CountryEntity.SeedDataAsync(ctx, cancellationToken);
            await ScrapeTypeEntity.SeedDataAsync(ctx, cancellationToken);
            await PositionEntity.SeedDataAsync(ctx, cancellationToken);
        });
    }
}