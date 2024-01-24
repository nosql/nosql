using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlLikeExpression : SqlExpression
{
    public SqlLikeExpression(TypeMapping typeMapping, SqlExpression expression, string value) : base(typeMapping)
    {
        Expression = expression;
        Value = value;
    }

    public SqlExpression Expression { get; }
    public string Value { get; }
}
