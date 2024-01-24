using NoSql.Storage;

namespace NoSql.Query.Expressions;

public abstract class SqlExpression
{
    public SqlExpression(Type type, TypeMapping? typeMapping)
    {
        Type = type;
        TypeMapping = typeMapping;
    }

    public SqlExpression(TypeMapping typeMapping)
    {
        Type = typeMapping.ClrType;
        TypeMapping = typeMapping;
    }


    public TypeMapping? TypeMapping { get; }

    public virtual Type Type { get; }
}
