using FutSpect.DAL.Entities.Clubs;
using FutSpect.DAL.Entities.Countries;
using FutSpect.DAL.Entities.Leagues;
using Microsoft.EntityFrameworkCore;
using Constants = FutSpect.Shared.Constants;

namespace FutSpect.DAL;

public class FutSpectContext(DbContextOptions<FutSpectContext> options) : DbContext(options)
{
    public DbSet<ClubEntity> Clubs { get; set; }

    public DbSet<CountryEntity> Countries { get; set; }

    public DbSet<ClubLogoEntity> ClubLogos { get; set; }

    public DbSet<LeagueEntity> Leagues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;");

        options.UseSeeding((ctx, _) =>
        {
            var usa = ctx.Set<CountryEntity>().FirstOrDefault(x => x.Id == Constants.Countries.USA);
            if (usa is null)
            {
                ctx.Set<CountryEntity>().Add(new CountryEntity
                {
                    Id = Constants.Countries.USA,
                    Name = "United States of America",
                    Abbreviation = "US"
                });

                ctx.SaveChanges();
            }
        });

        options.UseAsyncSeeding(async (ctx, _, cancellationToken) =>
        {
            var usa = ctx.Set<CountryEntity>().FirstOrDefault(x => x.Id == Constants.Countries.USA);
            if (usa is null)
            {
                ctx.Set<CountryEntity>().Add(new CountryEntity
                {
                    Id = Constants.Countries.USA,
                    Name = "United States of America",
                    Abbreviation = "US"
                });

                await ctx.SaveChangesAsync(cancellationToken);
            }
        });
    }
}
