using FutSpect.Dal.Interfaces;

namespace FutSpect.Api.Models;

public class LeaguesRequest : ISearchable
{
    public string? SearchTerm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}