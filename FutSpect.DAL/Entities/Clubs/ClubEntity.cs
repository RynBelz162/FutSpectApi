using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Entities.Leagues;
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

    public required int LeagueId { get; init; }

    [Unicode(false)]
    [MaxLength(150)]
    public string? RosterUrl { get; init; }

    [Unicode(false)]
    [MaxLength(150)]
    public string? ScheduleUrl { get; init; }

    [ForeignKey(nameof(LeagueId))]
    public LeagueEntity? League { get; init; }
}