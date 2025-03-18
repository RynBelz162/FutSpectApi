using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Entities.Lookups;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Leagues;

[Index(nameof(Name), nameof(CountryId), IsUnique = true)]
public class LeagueEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(100)]
    public required string Name { get; set;}

    [Required]
    public required int CountryId { get; init; }

    [ForeignKey(nameof(CountryId))]
    public CountryEntity Country { get; init; } = null!;
}