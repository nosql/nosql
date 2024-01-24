namespace NoSql;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class IndexAttribute : Attribute
{
    public IndexAttribute(params string[] columns)
    {
        Columns = columns;
        IsDescending = new bool[Columns.Length];
    }

    public IndexAttribute(string column, bool descending)
    {
        Columns = [column];
        IsDescending = [descending];
    }

    public IndexAttribute(string[] fields, bool[] descending)
    {
        Columns = fields;
        IsDescending = descending;
    }

    public string[] Columns { get; }

    public bool[] IsDescending { get; set; }
}
