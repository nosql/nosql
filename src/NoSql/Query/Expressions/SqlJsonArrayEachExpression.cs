namespace NoSql.Query.Expressions;

public class SqlJsonArrayEachExpression : SqlTableBaseExpression
{
    public SqlJsonArrayEachExpression(SqlExpression expression, string? alias = null)
        : base(expression.Type, expression.TypeMapping, alias)
    {
        Expression = expression;
    }

    public SqlExpression Expression { get; }
}
