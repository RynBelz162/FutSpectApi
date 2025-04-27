using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Clubs;

public class ClubLogoEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; init; }

    [Required]
    public required int ClubId { get; init; }

    [Required]
    public required byte[] Bytes { get; init; }

    public string? SrcUrl { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(25)]
    public required string Extension { get; init; }

    [ForeignKey(nameof(ClubId))]
    public ClubEntity? Club { get; init; }
}