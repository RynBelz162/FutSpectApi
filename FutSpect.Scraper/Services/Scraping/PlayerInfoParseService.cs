using System.Text.RegularExpressions;
using FutSpect.Shared.Constants;

namespace FutSpect.Scraper.Services.Scraping;

public partial class PlayerInfoParseService : IPlayerInfoParseService
{
    [GeneratedRegex("\\d+")]
    private static partial Regex NumberRegex();

    public (string? FirstName, string LastName) GetName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return (null, string.Empty);
        }

        string? firstName = null;
        string lastName;

        var names = value.Split(" ");

        if (names.Length > 1)
        {
            firstName = names.First();
            lastName = names.Last();
        }
        else
        {
            lastName = names.First();
        }

        return (firstName, lastName);
    }

    public int GetPositionId(string? value) =>
        value?.ToLower() switch
        {
            "forward" => Positions.Forward,
            "midfielder" => Positions.Midfielder,
            "defender" => Positions.Defender,
            "goalkeeper" => Positions.Goalkeeper,
            _ => Positions.Goalkeeper
        };

    public short GetNumber(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        var match = NumberRegex().Match(value);
        if (match.Success && short.TryParse(match.Value, out var numberRaw))
        {
            return numberRaw;
        }

        return 0;
    }

    public string GetCleanTextValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return string.Empty;
    }
}