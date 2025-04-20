namespace FutSpect.Shared.Models.Scraping;

public record ScrapeLedger
{
    public int Id { get; init; }
    public int LeagueId { get; init; }
    public int TypeId { get; init; }
}