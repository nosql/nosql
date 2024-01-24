using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlColumnExpression : SqlExpression
{
    public SqlColumnExpression(NoSqlFieldInfo column, string? tableAlias = null) : this(column.TypeMapping, column.Name)
    {
        TableAlias = tableAlias;
    }

    public SqlColumnExpression(Type type, TypeMapping? typeMapping, string columnName, string? tableAlias = null) : base(type, typeMapping)
    {
        Name = columnName;
        TableAlias = tableAlias;
    }

    public SqlColumnExpression(TypeMapping typeMapping, string columnName, string? tableAlias = null) : base(typeMapping.ClrType, typeMapping)
    {
        Name = columnName;
        TableAlias = tableAlias;
    }

    public string Name { get; }

    public string? TableAlias { get; }

    public SqlJsonExtractExpression Extract(TypeMapping typeMapping, params PathSegment[] path)
    {
        return new SqlJsonExtractExpression(typeMapping, new SqlColumnExpression(Type, TypeMapping, Name), path);
    }
}
