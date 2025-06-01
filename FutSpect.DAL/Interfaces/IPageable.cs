namespace FutSpect.DAL.Interfaces;

public interface IPageable
{
    /// <summary>
    /// Gets the current page number.
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Calculates the number of items to skip based on the current page and page size.
    /// </summary>
    int Skip => (Page - 1) * PageSize;
}