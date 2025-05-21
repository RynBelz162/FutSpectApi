using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Entities.Lookups;
using FutSpect.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Leagues;

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

    [Column(TypeName = "datetime2(2)")]
    public required DateTime CreatedDate { get; init; }

    [ForeignKey(nameof(CountryId))]
    public CountryEntity Country { get; init; } = null!;
}