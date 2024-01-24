namespace NoSql;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class PrimaryKeyAttribute(params string[] fields) : Attribute
{
    public string[] Fields { get; } = fields;
}