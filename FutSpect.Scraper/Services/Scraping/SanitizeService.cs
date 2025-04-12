namespace FutSpect.Scraper.Services.Scraping;

public class SanitizeService : ISanitizeService
{
    public string Sanitize(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        ReadOnlySpan<char> span = value;
        var noNewLines = RemoveNewLines(span);
        return RemoveExtraWhitespace(noNewLines);
    }

    private static string RemoveNewLines(ReadOnlySpan<char> span)
    {
        if (span.IsEmpty)
        {
            return string.Empty;
        }

        Span<char> result = stackalloc char[span.Length];
        var position = 0;

        foreach (var c in span)
        {
            if (c is not '\r' and not '\n')
            {
                result[position++] = c;
            }
        }

        return new string(result[..position]);
    }

    private static string RemoveExtraWhitespace(ReadOnlySpan<char> span)
    {
        if (span.IsEmpty)
        {
            return string.Empty;
        }

        Span<char> result = stackalloc char[span.Length];
        var position = 0;
        var previousWasSpace = true; // Treat start of string as space to trim leading spaces

        foreach (var c in span)
        {
            if (char.IsWhiteSpace(c))
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
}