namespace FutSpect.Scraper.Options;

public class BackgroundJobOptions
{
    public required string ClubScrapeCron { get; init; }
    public required string LeagueScrapeCron { get; init; }
}