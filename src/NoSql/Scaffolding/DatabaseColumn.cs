namespace NoSql.Scaffolding;

public class DatabaseColumn
{
    public DatabaseColumn(string name, string storeType, bool isNullable)
    {
        Name = name;
        StoreType = storeType;
        IsNullable = isNullable;
    }

    public string Name { get; }
    public string StoreType { get; }
    public bool IsNullable { get; }
}
