using Microsoft.Data.Sqlite;
using NoSql.Storage;
using System.Data.Common;

namespace NoSql.Sqlite.Storage;

public class SqliteDbConnection : RelationalDbConnection
{
    private readonly string _connectionString;
    private SqliteConnection? _connection;

    public SqliteDbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override DbConnection DbConnection
    {
        get
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection(_connectionString);
                _connection.CreateFunction("floor", (double value) => Math.Floor(value));
                _connection.CreateFunction("ceiling", (double value) => Math.Ceiling(value));
            }
            return _connection;
        }
    }
}