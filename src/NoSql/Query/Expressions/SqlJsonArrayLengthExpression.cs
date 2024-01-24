using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlJsonArrayLengthExpression : SqlExpression
{
    public SqlJsonArrayLengthExpression(TypeMapping typeMapping, SqlExpression expression) : base(typeMapping)
    {
        Expression = expression;
    }

    public SqlExpression Expression { get; }
}