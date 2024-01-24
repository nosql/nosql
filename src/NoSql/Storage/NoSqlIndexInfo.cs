namespace NoSql.Storage;

public class NoSqlIndexInfo
{
    public NoSqlIndexInfo(string name, IList<NoSqlFieldInfo> columns, IList<bool> descending)
    {
        Name = name;
        Columns = columns;
        IsDescending = descending;
    }

    public string Name { get; }
    public IList<NoSqlFieldInfo> Columns { get; }
    public bool IsUnique { get; }
    public IList<bool> IsDescending { get; }
}