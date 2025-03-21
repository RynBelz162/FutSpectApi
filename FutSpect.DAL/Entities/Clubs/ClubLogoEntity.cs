using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FutSpect.DAL.Entities.Clubs;

public class ClubLogoEntity
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public required int ClubId { get; init; }

    [Required]
    public required byte[] Bytes { get; init;}

    public string? SrcUrl { get; init; }

    [ForeignKey(nameof(ClubId))]
    public ClubEntity? Club { get; init; }
}