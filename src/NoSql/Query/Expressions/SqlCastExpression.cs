using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlCastExpression : SqlExpression
{
    public SqlCastExpression(Type type, TypeMapping typeMapping, SqlExpression expression) : base(type, typeMapping)
    {
        Expression = expression;
    }

    public SqlCastExpression(TypeMapping typeMapping, SqlExpression expression) : base(typeMapping)
    {
        Expression = expression;
    }

    public SqlExpression Expression { get; }
}