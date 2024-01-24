using NoSql.Scaffolding;
using NoSql.Storage;
using System.Text;

namespace NoSql.PostgreSql.Scaffolding;

public class PostgreSqlDatabaseFactory : RelationalDatabaseFactory
{
    public PostgreSqlDatabaseFactory(INoSqlDbConnection connection) : base(connection)
    {
    }

    protected override void GenerateColumnScript(StringBuilder builder, NoSqlFieldInfo column)
    {
        builder.Append($"\"{column.Name}\" ");

        if (column.AutoIncrement)
        {
            if (column.TypeMapping.StoreType == "integer")
            {
                builder.Append("SERIAL");
            }
            else if (column.TypeMapping.StoreType == "smallint")
            {
                builder.Append("SMALLSERIAL");
            }
            else if (column.TypeMapping.StoreType == "bigint")
            {
                builder.Append("BIGSERIAL");
            }
            else
            {
                throw new NotSupportedException($"not support auto increment column with store type '{column.TypeMapping.StoreType}'");
            }
        }
        else
        {
            builder.Append(column.TypeMapping.StoreType);

            if (!column.IsNullable)
            {
                builder.Append(" NOT NULL");
            }
        }
    }

    protected override DatabaseTable? GetTable(string table)
    {
        var connection = ((RelationalDbConnection)Connection).DbConnection;
        var command = connection.CreateCommand();
        command.CommandText = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM information_schema.columns WHERE TABLE_NAME = '{table}'";
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
