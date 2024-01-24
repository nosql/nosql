namespace NoSql.Query.Expressions;

public class SqlOrderingExpression : SqlExpression
{
    public SqlOrderingExpression(SqlExpression expression, bool descending) : base(expression.Type, null)
    {
        Expression = expression;
        IsDescending = descending;
    }

    public SqlExpression Expression { get; }
    public bool IsDescending { get; }
}