namespace FutSpect.Scraper.Models;

public record ScrapedImage
{
    public required string ImageSrcUrl { get; init; }
    public required byte[] ImageBytes { get; init; }
    public required string ImageExtension { get; init; }
}