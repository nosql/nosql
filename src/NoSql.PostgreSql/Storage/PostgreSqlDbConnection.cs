using NoSql.Storage;
using Npgsql;
using System.Data.Common;

namespace NoSql.PostgreSql.Storage;

public class PostgreSqlDbConnection : RelationalDbConnection
{
    private readonly NpgsqlDataSource _source;
    private DbConnection? _connection;

    public PostgreSqlDbConnection(string connectionString)
    {
        _source = new NpgsqlDataSourceBuilder(connectionString).Build();
    }

    public override DbConnection DbConnection => _connection ??= _source.CreateConnection();
}