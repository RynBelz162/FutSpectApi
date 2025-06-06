namespace FutSpect.Shared.Models.Leagues;

public record League
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Abbreviation { get; init; }
    public required int CountryId { get; init; }
    public required bool HasProRel { get; init; }
    public required short PyramidLevel { get; init; }
    public string CountryName { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public Guid? LogoId { get; init; }
}