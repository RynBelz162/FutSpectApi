using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.Dal.Entities.Leagues;
using FutSpect.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Dal.Entities.Clubs;

public class LeagueLogoEntity : IRecordable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; init; }

    [Required]
    public required int LeagueId { get; init; }

    [Required]
    public required byte[] Bytes { get; init; }

    public string? SrcUrl { get; init; }

    [Required]
    [Unicode(false)]
    [MaxLength(25)]
    public required string Extension { get; init; }

    public required DateTime CreatedOn { get; init; }

    public required DateTime ModifiedOn { get; init; }

    [ForeignKey(nameof(LeagueId))]
    public LeagueEntity? League { get; init; }
}