namespace FutSpect.Shared.Models.Clubs;

public record ClubInfo
{
    public required string Name { get; init; }
    public required int LeagueId { get; init; }
    public string? RosterUrl { get; init; }
    public string? ScheduleUrl { get; init; }
};