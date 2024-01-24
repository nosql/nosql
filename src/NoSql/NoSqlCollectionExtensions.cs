namespace NoSql;

public static class NoSqlCollectionExtensions
{
    public static int Insert<T>(this NoSqlCollection<T> query, IEnumerable<T> values) => query.Insert(values.ToArray());

    public static Task<int> InsertAsync<T>(this NoSqlCollection<T> query, IEnumerable<T> values, CancellationToken cancellationToken = default) => query.InsertAsync(values.ToArray(), cancellationToken);
}
