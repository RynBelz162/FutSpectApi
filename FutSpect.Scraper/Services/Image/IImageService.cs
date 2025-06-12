using FutSpect.Scraper.Models;
using FutSpect.Shared.Models.Result;

namespace FutSpect.Scraper.Services.Image;

public interface IImageService
{
    Task<Result<ScrapedImage>> DownloadImageAsync(string hostUrl, string? imageUrl);
}
