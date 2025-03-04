namespace FutSpect.Shared.Models.Leagues;

public class League
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required int CountryId { get; init; }
    public string CountryName { get; init;} = string.Empty;
}