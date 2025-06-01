using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.Dal.Entities.Leagues;
using FutSpect.Dal.Entities.Lookups;

namespace FutSpect.Dal.Entities.Scraping;

[Table("ScrapeLedger")]
public class ScrapeLedgerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    public required int TypeId { get; init; }

    [Required]
    public required int LeagueId { get; init; }

    [Required]
    public DateTime CreatedDate { get; init; }

    [ForeignKey(nameof(TypeId))]
    public ScrapeTypeEntity ScrapeType { get; init; } = null!;

    [ForeignKey(nameof(LeagueId))]
    public LeagueEntity League { get; init; } = null!;
}