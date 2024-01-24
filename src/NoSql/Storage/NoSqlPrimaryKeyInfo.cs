namespace NoSql.Storage;

public class NoSqlPrimaryKeyInfo
{
    public NoSqlPrimaryKeyInfo(string name, NoSqlFieldInfo[] columns)
    {
        Name = name;
        Columns = columns;
    }

    public string Name { get; }
    public NoSqlFieldInfo[] Columns { get; }
}
