namespace FutSpect.Shared.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable) where T : class =>
        enumerable.Where(x => x != null)!;
}