using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SharedConstants = FutSpect.Shared.Constants;

namespace FutSpect.DAL.Entities.Countries;

[Index(nameof(Name), IsUnique = true)]
public class CountryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; init; }

    [Required]
    [MaxLength(75)]
    public required string Name { get; init; }

    [Required]
    [MaxLength(2)]
    public required string Abbreviation { get; init; }

    public static void SeedData(DbContext dbContext)
    {
        var usa = dbContext
            .Set<CountryEntity>()
            .FirstOrDefault(x => x.Id == SharedConstants.Countries.USA);

        if (usa is null)
        {
            dbContext
                .Set<CountryEntity>()
                .Add
                (
                    new CountryEntity
                    {
                        Id = SharedConstants.Countries.USA,
                        Name = "United States of America",
                        Abbreviation = "US"
                    }
                );

            dbContext.SaveChanges();
        }
    }

    public static async Task SeedDataAsync(DbContext dbContext, CancellationToken cancellationToken)
    {
        var usa = await dbContext
            .Set<CountryEntity>()
            .FirstOrDefaultAsync(x => x.Id == SharedConstants.Countries.USA, cancellationToken: cancellationToken);

        if (usa is null)
        {
            dbContext
                .Set<CountryEntity>()
                .Add
                (
                    new CountryEntity
                    {
                        Id = SharedConstants.Countries.USA,
                        Name = "United States of America",
                        Abbreviation = "US"
                    }
                );

            await dbContext.SaveChangesAsync(cancellationToken);
        } 
    }
}