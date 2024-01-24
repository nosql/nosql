using NoSql.Query.Expressions;

namespace NoSql.Storage;

public class NoSqlTypeInfo
{
    private SqlProjectionListExpression? _projections;

    public NoSqlTypeInfo(string name, Type clrType, TypeMapping typeMapping)
    {
        Name = name;
        ClrType = clrType;
        TypeMapping = typeMapping;
        Fields = Array.Empty<NoSqlFieldInfo>();
        MappedFields = Array.Empty<NoSqlFieldInfo>();
        Indexes = Array.Empty<NoSqlIndexInfo>();
    }

    public NoSqlTypeInfo(
        string name,
        Type clrType,
        Func<object?[]?, object> createInstance,
        bool requireConstructorParameters,
        TypeMapping typeMapping,
        NoSqlFieldInfo[] fields,
        NoSqlPrimaryKeyInfo? primaryKey,
        NoSqlIndexInfo[] indexes)
    {
        Name = name;
        CreateInstance = createInstance;
        RequireConstructorParameters = requireConstructorParameters;
        PrimaryKey = primaryKey;
        ClrType = clrType;
        TypeMapping = typeMapping;
        Fields = fields;
        MappedFields = fields.Where(x => !x.IsNotMapped).ToArray();
        Indexes = indexes ?? Array.Empty<NoSqlIndexInfo>();
    }

    public string Name { get; }
    public Type ClrType { get; }
    public TypeMapping TypeMapping { get; }
    public NoSqlPrimaryKeyInfo? PrimaryKey { get; }
    public NoSqlFieldInfo[] Fields { get; }
    public NoSqlFieldInfo[] MappedFields { get; }
    public NoSqlIndexInfo[] Indexes { get; }
    public Func<object?[]?, object>? CreateInstance { get; }
    public bool RequireConstructorParameters { get; }

    public SqlProjectionListExpression Projections
    {
        get
        {
            if (_projections == null)
            {
                var projections = new SqlProjectionExpression[MappedFields.Length];
                for (int i = 0; i < MappedFields.Length; i++)
                {
                    var column = MappedFields[i];
                    projections[i] = new SqlProjectionExpression(new SqlColumnExpression(column), column.Name);
                }
                _projections = new SqlProjectionListExpression(TypeMapping, projections);
            }
            return _projections;
        }
    }


    public NoSqlFieldInfo GetMappedColumn(string fieldName)
    {
        var column = MappedFields.FirstOrDefault(x => string.Equals(x.FieldName, fieldName, StringComparison.OrdinalIgnoreCase));
        if (column == null)
        {
            ThrowHelper.ThrowTypeException_ColumnNotFound(Name, fieldName);
        }
        return column;
    }
}
