namespace NoSql.Storage;

public class NoSqlFieldInfo
{
    public NoSqlFieldInfo(
                        string name,
                        string filedName,
                        TypeMapping typeMapping,
                        bool unmapped,
                        bool nullable,
                        bool autoIncrement,
                        Func<object, object?>? getMethod,
                        Action<object, object?>? setMethod)
    {
        Name = name;
        FieldName = filedName;
        TypeMapping = typeMapping;
        IsNotMapped = unmapped;
        IsNullable = nullable;
        AutoIncrement = autoIncrement;
        Get = getMethod;
        Set = setMethod;
    }

    public string Name { get; }
    public string FieldName { get; }
    public bool IsNullable { get; }
    public bool IsNotMapped { get; }
    public TypeMapping TypeMapping { get; }
    public Func<object, object?>? Get { get; }
    public Action<object, object?>? Set { get; }

    public bool AutoIncrement { get; }
}