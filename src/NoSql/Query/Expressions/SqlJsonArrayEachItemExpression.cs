using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlJsonArrayEachItemExpression : SqlExpression
{
    public SqlJsonArrayEachItemExpression(Type type, TypeMapping? typeMapping) : base(type, typeMapping)
    {
    }

    public SqlJsonArrayEachItemExpression(TypeMapping typeMapping) : base(typeMapping)
    {
    }
}
