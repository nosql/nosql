using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlProjectionExpression : SqlExpression
{
    public SqlProjectionExpression(SqlExpression expression, string? alias = null) : base(expression.Type, expression.TypeMapping)
    {
        Expression = expression;
        Alias = alias;
    }

    public SqlExpression Expression { get; }

    public string? Alias { get; }
}

public class SqlProjectionListExpression : SqlExpression
{
    public SqlProjectionListExpression(TypeMapping typeMapping, SqlProjectionExpression[] projections) : base(typeMapping)
    {
        ProjectionList = projections;
    }

    public SqlProjectionListExpression(Type type, TypeMapping? typeMapping, SqlProjectionExpression[] projections) : base(type, typeMapping)
    {
        ProjectionList = projections;
    }

    public SqlProjectionExpression[] ProjectionList { get; }
}