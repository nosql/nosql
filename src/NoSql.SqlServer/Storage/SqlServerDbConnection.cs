using Microsoft.Data.SqlClient;
using NoSql.Storage;
using System.Data.Common;

namespace NoSql.SqlServer.Storage;

public class SqlServerDbConnection : RelationalDbConnection
{
    private readonly string _connectionString;
    private DbConnection? _connection;

    public SqlServerDbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override DbConnection DbConnection => _connection ??= new SqlConnection(_connectionString);
}