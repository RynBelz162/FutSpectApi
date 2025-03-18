using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Entities.Lookups;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Clubs;

[Index(nameof(Name), IsUnique = true)]
public class ClubEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(100)]
    public required string Name { get; init; }

    public required int CountryId { get; init; }

    [ForeignKey(nameof(CountryId))]
    public CountryEntity? Country { get; init; }

    [ForeignKey(nameof(ClubLogoEntity.ClubId))]
    public ClubLogoEntity? Logo { get; init; }
}