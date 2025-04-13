namespace FutSpect.Scraper.Services.Scraping;

public class SanitizeService : ISanitizeService
{
    public string Sanitize(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        ReadOnlySpan<char> span = value;
        Span<char> result = stackalloc char[span.Length];
        var position = 0;
        var previousWasSpace = true;

        foreach (var c in span)
        {
            if (ShouldReplaceCharacter(c))
            {
                if (!previousWasSpace)
                {
                    result[position++] = ' ';
                    previousWasSpace = true;
                }
            }
            else
            {
                result[position++] = c;
                previousWasSpace = false;
            }
        }

        // Trim trailing space if exists
        if (position > 0 && result[position - 1] == ' ')
        {
            position--;
        }

        return new string(result[..position]);
    }

    private static bool ShouldReplaceCharacter(char c) => c is '\r' or '\n' || char.IsWhiteSpace(c);
}