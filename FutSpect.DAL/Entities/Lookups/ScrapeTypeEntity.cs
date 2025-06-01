using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.Dal.Constants;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Dal.Entities.Lookups;

[Index(nameof(Name), IsUnique = true)]
public class ScrapeTypeEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [Required]
    [MaxLength(75)]
    public required string Name { get; init; }

    private static ScrapeTypeEntity[] DefaultValues =>
    [
        new ScrapeTypeEntity
        {
            Id = ScrapeTypes.LeagueInfo,
            Name = "League Info",
        },
        new ScrapeTypeEntity
        {
            Id = ScrapeTypes.ClubInfo,
            Name = "Club Info",
        },
    ];

    private static int[] TypeIds => [ScrapeTypes.LeagueInfo, ScrapeTypes.ClubInfo];

    public static void SeedData(DbContext dbContext)
    {
        var entityCount = dbContext
            .Set<ScrapeTypeEntity>()
            .Count(x => TypeIds.Contains(x.Id));

        if (entityCount != DefaultValues.Length)
        {
            dbContext
                .Set<ScrapeTypeEntity>()
                .AddRange(DefaultValues);

            dbContext.SaveChanges();
        }
    }

    public static async Task SeedDataAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var entityCount = await dbContext
            .Set<ScrapeTypeEntity>()
            .CountAsync(x => TypeIds.Contains(x.Id));

        if (entityCount != DefaultValues.Length)
        {
            await dbContext
                .Set<ScrapeTypeEntity>()
                .AddRangeAsync(DefaultValues);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}