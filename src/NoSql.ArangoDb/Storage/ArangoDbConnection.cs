using NoSql.Storage;
using System.Runtime.CompilerServices;

namespace NoSql.ArangoDb.Storage;

public class ArangoDbConnection : INoSqlDbConnection
{
    private readonly NoSqlOptions _options;
    private readonly string _clientName;
    private readonly IHttpClientFactory _clientFactory;

    public ArangoDbConnection(IHttpClientFactory clientFactory, NoSqlOptions options, ArangoOptions arangoOptions)
    {
        _clientFactory = clientFactory;
        _options = options;
        _clientName = "nosql_arangodb_" + arangoOptions.Database;
    }

    internal HttpClient CreateHttpClient() => _clientFactory.CreateClient(_clientName);

    public int ExecuteNonQuery(string sql) => ExecuteNonQueryAsync(sql, CancellationToken.None).Result;
    public T? ExecuteOneOrDefaultOne<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields) => ExecuteOneOrDefaultAsync<T>(sql, tableInfo, fields, CancellationToken.None).Result;
    public T? ExecuteScalar<T>(string sql) => ExecuteScalarAsync<T>(sql, CancellationToken.None).Result;

    public IEnumerable<T?> ExecuteEnumerable<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields)
    {
        ArangoQueryList<T> result;

        var httpClient = CreateHttpClient();
        result = httpClient.CursorAsync<T>(sql, new AqlQueryOptions
        {
            Count = false,
        }, _options.SerializerOptions, CancellationToken.None).Result;

        foreach (var r in result.Result)
        {
            yield return r;
        }

        while (result.HasMore)
        {
            result = httpClient.CursorNextAsync<T>(result.Next!, _options.SerializerOptions, CancellationToken.None).Result;

            foreach (var r in result.Result)
            {
                yield return r;
            }
        }
    }

    public async IAsyncEnumerable<T?> ExecuteEnumerableAsync<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ArangoQueryList<T> result;

        var httpClient = CreateHttpClient();
        result = await httpClient.CursorAsync<T>(sql, new AqlQueryOptions
        {
            Count = false,
        }, _options.SerializerOptions, cancellationToken);

        foreach (var r in result.Result)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return r;
        }

        while (result.HasMore)
        {
            result = await httpClient.CursorNextAsync<T>(result.Next!, _options.SerializerOptions, cancellationToken);

            foreach (var r in result.Result)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return r;
            }
        }
    }

    public async Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken)
    {
        var result = await CreateHttpClient().CursorAsync<int>(sql, new AqlQueryOptions
        {
            Count = false,
            BatchSize = 1,
        }, _options.SerializerOptions, cancellationToken);

        return result.Extra?.Stats?.WritesExecuted ?? 1;
    }

    public Task<T?> ExecuteOneOrDefaultAsync<T>(string sql, NoSqlTypeInfo tableInfo, NoSqlFieldInfo[]? fields, CancellationToken cancellationToken)
    {
        return CreateHttpClient().FindOneOrDefaultAsync<T>(sql, _options.SerializerOptions, cancellationToken);
    }

    public async Task<T?> ExecuteScalarAsync<T>(string sql, CancellationToken cancellationToken)
    {
        var result = await CreateHttpClient().CursorAsync<T>(sql, new AqlQueryOptions
        {
            Count = false,
            BatchSize = 1,
        }, _options.SerializerOptions, cancellationToken);

        return result.Result.FirstOrDefault();
    }
}
