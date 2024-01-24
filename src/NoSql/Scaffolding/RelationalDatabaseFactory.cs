using NoSql.Storage;
using System.Text;

namespace NoSql.Scaffolding;

public class RelationalDatabaseFactory : IDatabaseFactory
{
    protected readonly INoSqlDbConnection Connection;

    public RelationalDatabaseFactory(INoSqlDbConnection connection)
    {
        Connection = connection;
    }

    public TableCreateResult CreateTable(NoSqlTypeInfo table)
    {
        var original = GetTable(table.Name);
        if (original != null)
        {
            return MigrateTable(original, table) ? TableCreateResult.Migrated : TableCreateResult.None;
        }

        StringBuilder builder = new();
        GenerateCreateTableScript(builder, table);

        Connection.ExecuteNonQuery(builder.ToString());
        return TableCreateResult.Created;
    }

    public async Task<TableCreateResult> CreateTableAsync(NoSqlTypeInfo table, CancellationToken cancellationToken)
    {
        var original = GetTable(table.Name);
        if (original != null)
        {
            return await MigrateTableAsync(original, table, cancellationToken) ? TableCreateResult.Migrated : TableCreateResult.None;
        }

        StringBuilder builder = new();
        GenerateCreateTableScript(builder, table);
        await Connection.ExecuteNonQueryAsync(builder.ToString(), cancellationToken);
        return TableCreateResult.Created;
    }

    public bool MigrateTable(DatabaseTable original, NoSqlTypeInfo table)
    {
        var additionals = table.MappedFields
            .Where(x => original.Columns.All(y => !string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (additionals.Count > 0)
        {
            var script = GenerateMigrateTableScript(table, additionals);

            Connection.ExecuteNonQuery(script);
            return true;
        }

        return false;
    }

    public async Task<bool> MigrateTableAsync(DatabaseTable original, NoSqlTypeInfo table, CancellationToken cancellationToken)
    {
        var additionals = table.MappedFields
            .Where(x => original.Columns.All(y => !string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (additionals.Count > 0)
        {
            var script = GenerateMigrateTableScript(table, additionals);

            await Connection.ExecuteNonQueryAsync(script, cancellationToken);
            return true;
        }

        return false;
    }

    public bool DropTable(string tableName)
    {
        var original = GetTable(tableName);
        if (original == null)
        {
            return false;
        }

        Connection.ExecuteNonQuery(GenerateDropTableScript(tableName));
        return true;
    }


    public async Task<bool> DropTableAsync(string tableName, CancellationToken cancellationToken = default)
    {
        var original = GetTable(tableName);
        if (original == null)
        {
            return false;
        }

        await Connection.ExecuteNonQueryAsync(GenerateDropTableScript(tableName), cancellationToken);
        return true;
    }

    protected virtual string GenerateDropTableScript(string tableName)
    {
        return $"DROP TABLE \"{tableName}\"";
    }

    protected virtual string GenerateMigrateTableScript(NoSqlTypeInfo table, List<NoSqlFieldInfo> additionals)
    {
        StringBuilder builder = new();
        foreach (var column in additionals)
        {
            if (column.IsNotMapped)
            {
                continue;
            }
            builder.Append($"ALTER TABLE \"{table.Name}\" ADD COLUMN ");

            GenerateColumnScript(builder, column);

            builder.Append(";");
            builder.AppendLine();
        }

        return builder.ToString();
    }

    protected virtual void GenerateCreateTableScript(StringBuilder builder, NoSqlTypeInfo table)
    {
        builder.Append($"CREATE TABLE \"{table.Name}\"(");
        NoSqlFieldInfo column;
        for (int i = 0; i < table.MappedFields.Length; i++)
        {
            column = table.MappedFields[i];

            GenerateColumnScript(builder, column);

            if (i < table.MappedFields.Length - 1)
            {
                builder.Append(",");
            }
            else if (table.PrimaryKey != null)
            {
                builder.Append(", PRIMARY KEY (");

                for (int j = 0; j < table.PrimaryKey.Columns.Length; j++)
                {
                    column = table.PrimaryKey.Columns[j];
                    builder.Append($"\"{column.Name}\"");
                    if (j < table.PrimaryKey.Columns.Length - 1)
                    {
                        builder.Append(",");
                    }
                }
                builder.Append(")");
            }
        }

        builder.Append(");");

        foreach (var index in table.Indexes)
        {
            builder.AppendLine();
            GenerateCreateIndexScript(builder, table, index);
        }
    }

    protected virtual void GenerateColumnScript(StringBuilder builder, NoSqlFieldInfo column)
    {
        builder.Append($"\"{column.Name}\" ");
        builder.Append(column.TypeMapping.StoreType);

        if (!column.IsNullable)
        {
            builder.Append(" NOT NULL");
        }
    }

    protected virtual void GenerateCreateIndexScript(StringBuilder stringBuilder, NoSqlTypeInfo table, NoSqlIndexInfo index)
    {
        if (index.IsUnique)
        {
            stringBuilder.Append("CREATE UNIQUE INDEX ");
        }
        else
        {
            stringBuilder.Append("CREATE INDEX ");
        }
        stringBuilder.Append($"\"{index.Name}\" ON \"{table.Name}\"(");

        for (int i = 0; i < index.Columns.Count; i++)
        {
            stringBuilder.Append($"\"{index.Columns[i].Name}\"");
            if (index.IsDescending[i])
            {
                stringBuilder.Append(" DESC");
            }

            if (i < index.Columns.Count - 1)
            {
                stringBuilder.Append(",");
            }
        }
        stringBuilder.Append(");");
    }

    protected virtual DatabaseTable? GetTable(string table) => null;
}