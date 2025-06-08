namespace FutSpect.Dal.Interfaces;

public interface IPageable
{
    int Page { get; }

    int PageSize { get; }

    int Skip => (Page - 1) * PageSize;
}