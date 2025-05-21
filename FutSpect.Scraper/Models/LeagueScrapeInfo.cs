namespace FutSpect.Scraper.Models;

public class LeagueScrapeInfo
{
    public required string Name { get; init; }
    public required ScrapedImage Image { get; init; }
    public required int CountryId { get; init; }
    public required string Website { get; init; }
    public required bool HasProRel { get; init; }
    public required short PyramidLevel { get; init; }
    public required string Abbreviation { get; init; }
}