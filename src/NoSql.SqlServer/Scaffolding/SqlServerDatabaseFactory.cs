using NoSql.Scaffolding;
using NoSql.Storage;
using System.Text;

namespace NoSql.SqlServer.Scaffolding;

public class SqlServerDatabaseFactory : RelationalDatabaseFactory
{
    public SqlServerDatabaseFactory(INoSqlDbConnection connection) : base(connection)
    {
    }

    protected override void GenerateColumnScript(StringBuilder builder, NoSqlFieldInfo column)
    {
        builder.Append($"\"{column.Name}\" ");
        builder.Append(column.TypeMapping.StoreType);

        if (column.AutoIncrement)
        {
            builder.Append(" IDENTITY(1,1)");
        }
        else if (!column.IsNullable)
        {
            builder.Append(" NOT NULL");
        }
    }

    protected override DatabaseTable? GetTable(string table)
    {
        var connection = ((RelationalDbConnection)Connection).DbConnection;
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}'";
        connection.Open();

        List<DatabaseColumn> columns = new();

        try
        {
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var name = reader.GetString(0);
                var type = reader.GetString(1);
                var nullable = reader.GetString(2);

                var column = new DatabaseColumn(name, type, string.Equals(nullable, "YES", StringComparison.OrdinalIgnoreCase));
                columns.Add(column);
            }

            if (columns.Count == 0)
            {
                return null;
            }

            return new DatabaseTable(table, null, columns);
        }
        finally
        {
            connection.Close();
        }
    }

}
