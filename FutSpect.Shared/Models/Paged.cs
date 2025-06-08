using System.ComponentModel;

namespace FutSpect.Shared.Models;

[Description("Represents a paged collection of items.")]
public class Paged<T>
{
    [Description("The current page number.")]
    public int Page { get; set; }

    [Description("The number of items per page.")]
    public int PageSize { get; set; }

    [Description("The items available on the given page.")]
    public IEnumerable<T> Items { get; set; } = [];
}