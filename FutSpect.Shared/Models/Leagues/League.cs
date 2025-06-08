using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FutSpect.Shared.Models.Leagues;

public record League
{
    [Description("Unique identifier for the league.")]
    public int Id { get; init; }

    [Description("The name of the league (e.g., English Premier League).")]
    public required string Name { get; init; }

    [Description("The abbreviation for the league (e.g., EPL for English Premier League).")]
    public required string Abbreviation { get; init; }

    [Description("Indicates if the league has promotion and relegation (true) or not (false).")]
    public required bool HasProRel { get; init; }

    [Description("The level of the league in the pyramid (1 = top tier, 2 = second tier, etc.).")]
    public required short PyramidLevel { get; init; }

    [Description("The name of the country the league belongs to.")]
    public string CountryName { get; init; } = string.Empty;

    [Description("The official website for the league.")]
    public string Website { get; init; } = string.Empty;

    [Description("The unique identifier for the league logo.")]
    public Guid? LogoId { get; init; }

    [JsonIgnore]
    public int CountryId { get; init; }
}