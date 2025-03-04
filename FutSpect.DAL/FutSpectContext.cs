using FutSpect.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Constants = FutSpect.Shared.Constants;

namespace FutSpect.DAL;

public class FutSpectContext : DbContext
{
    public DbSet<Club> Clubs { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<ClubLogo> ClubLogos { get; set; }

    public DbSet<League> Leagues { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql($"Server=127.0.0.1;Port=5432;Database=FutSpect;User Id=postgres;Password=postgres;");

        options.UseSeeding((ctx, _) =>
        {
            var usa = ctx.Set<Country>().FirstOrDefault(x => x.Id == Constants.Countries.USA);
            if (usa is null)
            {
                ctx.Set<Country>().Add(new Country
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
            var usa = ctx.Set<Country>().FirstOrDefault(x => x.Id == Constants.Countries.USA);
            if (usa is null)
            {
                ctx.Set<Country>().Add(new Country
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
