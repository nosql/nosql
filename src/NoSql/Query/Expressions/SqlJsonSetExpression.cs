namespace NoSql.Query.Expressions;

public sealed class SqlJsonSetExpression : SqlExpression
{
    public SqlJsonSetExpression(SqlExpression expression, PathSegment[] path, SqlExpression value) : base(expression.Type, expression.TypeMapping)
    {
        Expression = expression;
        Path = path;
        Value = value;
    }

    public SqlExpression Expression { get; }

    public PathSegment[] Path { get; }

    public SqlExpression Value { get; }
}
