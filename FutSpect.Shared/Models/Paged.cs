namespace FutSpect.Shared.Models;

public class Paged<T>
{
    /// <summary>
    /// The current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The items on the current page.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = [];
}