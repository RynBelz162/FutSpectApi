using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.Dal.Entities.Lookups;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Dal.Entities.Players;

public class PlayerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Unicode(true)]
    [MaxLength(100)]
    public string? FirstName { get; init; }

    [Required]
    [Unicode(true)]
    [MaxLength(100)]
    public required string LastName { get; init; }

    [Required]
    public required int PositionId { get; init; }

    [Required]
    [MinLength(0)]
    [MaxLength(100)]
    public required short Number { get; init; }

    [ForeignKey(nameof(PositionId))]
    public PositionEntity? Position { get; init; }
}