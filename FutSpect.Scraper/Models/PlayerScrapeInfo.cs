namespace FutSpect.Scraper.Models;

public record PlayerScrapeInfo
{
    public string? FirstName { get; init; }
    public required string LastName { get; init; }
    public required int PositionId { get; init; }
    public required short Number { get; init; }
    public string? Birthplace { get; init; }
    public required ScrapedImage Image { get; init; }
}