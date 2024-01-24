using System.Collections.ObjectModel;

namespace NoSql.Scaffolding;

public class DatabasePrimaryKey
{
    public DatabasePrimaryKey(string name, IList<DatabaseColumn> columns)
    {
        Name = name;
        Columns = new ReadOnlyCollection<DatabaseColumn>(columns);
    }

    public string Name { get; }

    public IReadOnlyList<DatabaseColumn> Columns { get; }
}