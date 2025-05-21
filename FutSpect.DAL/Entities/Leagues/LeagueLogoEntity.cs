using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Entities.Leagues;
using FutSpect.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Entities.Clubs;

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

    [Column(TypeName = "datetime2(2)")]
    public required DateTime CreatedDate { get; init; }

    [ForeignKey(nameof(LeagueId))]
    public LeagueEntity? League { get; init; }
}