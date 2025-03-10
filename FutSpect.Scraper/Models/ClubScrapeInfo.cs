namespace FutSpect.Scraper.Models;

public class ClubScrapeInfo
{
    public required string Name { get; init; }
    public required string ImageSrcUrl { get; init; }
    public required string RosterUrl { get; init; }
    public required string ScheduleUrl { get; init; }
}