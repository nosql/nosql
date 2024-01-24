namespace NoSql;

public class NoSqlDatabase(NoSqlDependencies dependencies)
{
    public NoSqlDependencies Dependencies { get; } = dependencies;

    public virtual NoSqlCollection<T> Collection<T>(string? collectionName = null) => new(Dependencies, collectionName);
}