namespace NoSql.Extensions;

internal static class AsyncEnumerableExtensions
{
    public static async Task<List<T?>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable, CancellationToken cancellationToken = default)
    {
        List<T?> list = new();
        await foreach (var item in enumerable)
        {
            cancellationToken.ThrowIfCancellationRequested();
            list.Add(item);
        }
        return list;
    }
}
