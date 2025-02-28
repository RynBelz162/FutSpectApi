namespace FutSpect.Scrapper.Services;

public static class ImageDownloaderService
{
    public static async Task<(byte[] Bytes, string Extension)> DownloadImageAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        var uri = new Uri(imageUrl);

        // Get the file extension
        var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
        var fileExtension = Path.GetExtension(uriWithoutQuery);

        var bytes = await httpClient.GetByteArrayAsync(uri);
        return (bytes, fileExtension);
    }
}