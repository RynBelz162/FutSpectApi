namespace FutSpect.Shared.Models.Leagues;

public record LeagueLogo
{
    public required int LeagueId { get; init; }
    public required string ImageSrc { get; init; }
    public required byte[] ImageBytes { get; init; }
    public required string FileExtension { get; init; }
}