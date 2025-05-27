using FutSpect.Scraper.Models;

namespace FutSpect.Scraper.Options;

public class BackgroundJobOptions
{
    public required string ClubScrapeCron { get; init; }
    public required string LeagueScrapeCron { get; init; }
    public ScraperArgs ScraperArgs { get; set; } = new(false, false);
}