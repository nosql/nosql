namespace NoSql.Storage;

public interface INoSqlDbConnection
{
    T? ExecuteOneOrDefaultOne<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields);

    Task<T?> ExecuteOneOrDefaultAsync<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields, CancellationToken cancellationToken);
    
    IEnumerable<T?> ExecuteEnumerable<T>(string sql,
                                         NoSqlTypeInfo tableInfo,
                                         NoSqlFieldInfo[]? fields);

    IAsyncEnumerable<T?> ExecuteEnumerableAsync<T>(string sql,
                                                   NoSqlTypeInfo tableInfo,
                                                   NoSqlFieldInfo[]? fields,
                                                   CancellationToken cancellationToken);

    int ExecuteNonQuery(string sql);
    Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken);

    T? ExecuteScalar<T>(string sql);
    Task<T?> ExecuteScalarAsync<T>(string sql, CancellationToken cancellationToken);
}
