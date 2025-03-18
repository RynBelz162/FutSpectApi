using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FutSpect.DAL.Entities.Lookups;

namespace FutSpect.DAL.Entities.Scraping;

public class ScrapeLedgerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }

    [Required]
    public int TypeId { get; init; }

    [Required]
    public int LeagueId { get; init; }

    [Required]
    public DateTime CreatedDate { get; init; }

    [ForeignKey(nameof(TypeId))]
    public ScrapeTypeEntity ScrapeType { get; init; } = null!;
}