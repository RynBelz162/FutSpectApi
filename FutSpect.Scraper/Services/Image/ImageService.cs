using FutSpect.Scraper.Models;
using FutSpect.Shared.Models.Result;
using ImageMagick;

namespace FutSpect.Scraper.Services.Image;

public class ImageService : IImageService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ImageService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<ScrapedImage>> DownloadImageAsync(string hostUrl, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Result<ScrapedImage>.Fail("Image URL cannot be null or empty.");
        }

        using var httpClient = _httpClientFactory.CreateClient();

        var uri = GetDownloadUri(hostUrl, imageUrl);

        var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
        var fileExtension = Path.GetExtension(uriWithoutQuery);

        var bytes = await httpClient.GetByteArrayAsync(uri);

        var (compressedBytes, newFileExtension) = CompressImage(bytes, fileExtension);

        var result = new ScrapedImage
        {
            ImageBytes = compressedBytes,
            ImageExtension = newFileExtension,
            ImageSrcUrl = uri.ToString()
        };

        return Result<ScrapedImage>.Ok(result);
    }

    private static Uri GetDownloadUri(string hostUrl, string imageUrl)
    {
        if (imageUrl.StartsWith('/'))
        {
            Uri uri = new(hostUrl);
            return new Uri($"{uri.Scheme}://{uri.Host}{imageUrl}");
        }

        return new Uri(imageUrl);
    }

    public static (byte[], string fileExtension) CompressImage(byte[] bytes, string fileExtension)
    {
        const string JpgFileExtension = ".jpeg";
        using var image = new MagickImage(bytes);

        // Resize image if it's larger than 800px on either dimension
        if (image.Width > 800 || image.Height > 800)
        {
            var size = new MagickGeometry(800, 800)
            {
                IgnoreAspectRatio = false, // Maintain aspect ratio
                Greater = true // Only shrink images, don't enlarge smaller ones
            };
            image.Resize(size);
        }

        // Convert to JPEG if not already a JPEG
        if (!string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(fileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
        {
            image.Format = MagickFormat.Jpeg;
            fileExtension = JpgFileExtension;
        }

        // Set JPEG quality and apply lossless optimization
        image.Quality = 85;
        image.Strip(); // Remove any metadata to reduce size
        image.Settings.SetDefine(MagickFormat.Jpeg, "optimize-coding", "true"); // Enable lossless optimization
        image.Settings.SetDefine(MagickFormat.Jpeg, "dct-method", "float"); // Use floating-point DCT method for better precision
        image.Settings.Interlace = Interlace.NoInterlace; // Disable interlacing for smaller file size

        return (image.ToByteArray(), JpgFileExtension);
    }
}