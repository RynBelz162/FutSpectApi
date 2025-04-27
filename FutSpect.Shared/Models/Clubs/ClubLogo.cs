namespace FutSpect.Shared.Models.Clubs;

public record ClubLogo
{
    public required int ClubId { get; init; }
    public required string ImageSrc { get; init; }
    public required byte[] ImageBytes { get; init; }
    public required string FileExtension { get; init; }
}