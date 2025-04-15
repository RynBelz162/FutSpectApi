using System.Text;

namespace FutSpect.Scraper.Services.Scraping;

public class SanitizeService : ISanitizeService
{
    private static readonly HashSet<string> SoccerAbbreviations = new(StringComparer.OrdinalIgnoreCase)
    {
        "FC", "SC"
    };

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

    public string ToTitleCase(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var words = value.Split([' ', '-', '_'], StringSplitOptions.RemoveEmptyEntries);
        if (words.Length == 0) 
        {
            return string.Empty;
        }

        var sb = new StringBuilder();
        
        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (i != 0)
            {
                sb.Append(' ');
            }
            
            if (SoccerAbbreviations.Contains(word))
            {
                sb.Append(word.ToUpper());
            }
            else
            {
                sb.Append(char.ToUpper(word[0]) + word[1..].ToLower());
            }
        }

        return sb.ToString();
    }
}