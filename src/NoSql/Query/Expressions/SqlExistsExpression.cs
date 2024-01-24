using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlExistsExpression : SqlExpression
{
    public SqlExistsExpression(TypeMapping typeMapping, SqlExpression expression, bool isNegated = false) : base(typeMapping)
    {
        Expression = expression;
        IsNegated = isNegated;
    }

    public bool IsNegated { get; }

    public SqlExpression Expression { get; }
}