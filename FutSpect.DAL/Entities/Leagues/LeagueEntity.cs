using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.Dal.Entities.Lookups;
using FutSpect.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Dal.Entities.Leagues;

[Index(nameof(Name), nameof(CountryId), IsUnique = true)]
public class LeagueEntity : IRecordable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(100)]
    public required string Name { get; set; }

    [Required]
    [Unicode(false)]
    [MaxLength(10)]
    public required string Abbreviation { get; init; }

    [Required]
    [Range(1, 10)]
    public required short PyramidLevel { get; init; }

    public bool HasProRel { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(100)]
    public required string Website { get; set; }

    [Required]
    public required int CountryId { get; init; }

    public required DateTime CreatedOn { get; init; }

    public required DateTime ModifiedOn { get; init; }

    [ForeignKey(nameof(CountryId))]
    public CountryEntity Country { get; init; } = null!;
}