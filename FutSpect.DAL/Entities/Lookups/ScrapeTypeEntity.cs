using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Constants;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Lookups;

[Index(nameof(Name), IsUnique = true)]
public class ScrapeTypeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [Required]
    [MaxLength(75)]
    public required string Name { get; init; }

    public static void SeedData(DbContext dbContext)
    {
        var leagueInfoType = dbContext.Set<ScrapeTypeEntity>().FirstOrDefault(x => x.Id == ScrapeTypes.LeagueInfo);
        if (leagueInfoType is null)
        {
            dbContext
                .Set<ScrapeTypeEntity>()
                .Add
                (
                    new ScrapeTypeEntity
                    {
                        Id = 1,
                        Name = "League Info"
                    }
                );

            dbContext.SaveChanges();
        }
    }

    public static async Task SeedDataAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var leagueInfoType = await dbContext
            .Set<ScrapeTypeEntity>()
            .FirstOrDefaultAsync(x => x.Id == ScrapeTypes.LeagueInfo, cancellationToken: cancellationToken);

        if (leagueInfoType is null)
        {
            dbContext
                .Set<ScrapeTypeEntity>()
                .Add
                (
                    new ScrapeTypeEntity
                    {
                        Id = 1,
                        Name = "League Info"
                    }
                );

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}