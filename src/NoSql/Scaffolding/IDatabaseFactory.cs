using NoSql.Storage;

namespace NoSql.Scaffolding;

public interface IDatabaseFactory
{
    TableCreateResult CreateTable(NoSqlTypeInfo table);
    bool DropTable(string tableName);
    Task<TableCreateResult> CreateTableAsync(NoSqlTypeInfo table, CancellationToken cancellationToken = default);
    Task<bool> DropTableAsync(string tableName, CancellationToken cancellationToken = default);
}

public enum TableCreateResult
{
    None,
    Created,
    Migrated,
}