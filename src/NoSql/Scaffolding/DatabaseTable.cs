using System.Collections.ObjectModel;

namespace NoSql.Scaffolding;

public class DatabaseTable
{
    public DatabaseTable(string name, DatabasePrimaryKey? primaryKey, IList<DatabaseColumn> columns)
    {
        Name = name;
        PrimaryKey = primaryKey;
        Columns = new ReadOnlyCollection<DatabaseColumn>(columns);
    }

    public string Name { get; }

    public IReadOnlyList<DatabaseColumn> Columns { get; } 

    public DatabasePrimaryKey? PrimaryKey { get; }
}
