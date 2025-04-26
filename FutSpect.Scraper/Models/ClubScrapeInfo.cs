namespace FutSpect.Scraper.Models;

public class ClubScrapeInfo
{
    public required string Name { get; init; }
    public required int LeagueId { get; init; }
    public required ScrapedImage Image { get; init; }
    public required string RosterUrl { get; init; }
    public required string ScheduleUrl { get; init; }
}