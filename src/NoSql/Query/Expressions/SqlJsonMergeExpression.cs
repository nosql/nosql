namespace NoSql.Query.Expressions;

public sealed class SqlJsonMergeExpression : SqlExpression
{
    public SqlJsonMergeExpression(SqlExpression expression, SqlJsonObjectExpression value) : base(expression.Type, expression.TypeMapping)
    {
        Expression = expression;
        Value = value;
    }

    public SqlExpression Expression { get; }

    public SqlJsonObjectExpression Value { get; }
}
