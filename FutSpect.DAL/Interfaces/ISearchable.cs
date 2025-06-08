namespace FutSpect.Dal.Interfaces;

public interface ISearchable : IPageable
{
    string? SearchTerm { get; }
}