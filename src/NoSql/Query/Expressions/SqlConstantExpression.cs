using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlConstantExpression : SqlExpression
{
    public SqlConstantExpression(Type type, TypeMapping? typeMapping, object? value) : base(type, typeMapping)
    {
        Value = value;
    }

    public SqlConstantExpression(TypeMapping typeMapping, object? value) : base(typeMapping.ClrType, typeMapping)
    {
        Value = value;
    }

    public virtual object? Value { get; }
}
