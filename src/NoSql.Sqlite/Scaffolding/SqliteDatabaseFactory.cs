using NoSql.Scaffolding;
using NoSql.Storage;

namespace NoSql.Sqlite.Scaffolding;

public class SqliteDatabaseFactory : RelationalDatabaseFactory
{
    public SqliteDatabaseFactory(INoSqlDbConnection connection) :base(connection)
    {
    }

    protected override DatabaseTable? GetTable(string table)
    {
        var connection = ((RelationalDbConnection)Connection).DbConnection;
        var command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info(\"{table}\")";
        connection.Open();

        List<DatabaseColumn> columns = new();
        Dictionary<int, DatabaseColumn> keyOrders = new();
        try
        {
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var name = reader.GetString(1);
                var type = reader.GetString(2);
                var notnull = reader.GetBoolean(3);
                var pk = reader.GetInt32(5);

                var column = new DatabaseColumn(name, type, !notnull);
                columns.Add(column);

                if (pk > 0)
                {
                    keyOrders.Add(pk, column);
                }
            }

            if (columns.Count == 0)
            {
                return null;
            }

            List<DatabaseColumn>? key = null;
            if (keyOrders.Count > 0)
            {
                key = new(keyOrders.Count);
                foreach (var order in keyOrders.Keys.OrderBy(x => x))
                {
                    key.Add(keyOrders[order]);
                }
            }

            return new DatabaseTable(table,
                                        key == null ? null : new DatabasePrimaryKey($"PK_{table}", key),
                                        columns);
        }
        finally
        {
            connection.Close();
        }
    }
}
