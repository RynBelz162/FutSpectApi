using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.Shared.Constants;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Lookups;

public class PositionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(10)]
    public required string Name { get; init; }

    public static PositionEntity[] DefaultValues =>
    [
        new PositionEntity
        {
            Id = Positions.Goalkeeper,
            Name = "Goalkeeper",
        },
        new PositionEntity
        {
            Id = Positions.Defender,
            Name = "Defender",
        },
        new PositionEntity
        {
            Id = Positions.Midfielder,
            Name = "Midfielder",
        },
        new PositionEntity
        {
            Id = Positions.Forward,
            Name = "Forward",
        },
    ];

    public static int[] PositionIds => [Positions.Goalkeeper, Positions.Defender, Positions.Forward, Positions.Midfielder];

    public static void SeedData(DbContext dbContext)
    {
        var entityCount = dbContext
            .Set<PositionEntity>()
            .Count(x => PositionIds.Contains(x.Id));

        if (entityCount != DefaultValues.Length)
        {
            dbContext
                .Set<PositionEntity>()
                .AddRange(DefaultValues);

            dbContext.SaveChanges();
        }
    }

    public static async Task SeedDataAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var entityCount = await dbContext
            .Set<PositionEntity>()
            .CountAsync(x => PositionIds.Contains(x.Id));

        if (entityCount != DefaultValues.Length)
        {
            await dbContext
                .Set<PositionEntity>()
                .AddRangeAsync(DefaultValues);

            dbContext.SaveChanges();
        }
    } 
}