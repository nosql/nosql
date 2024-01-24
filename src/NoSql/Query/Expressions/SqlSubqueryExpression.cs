using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlSubqueryExpression : SqlExpression
{
    public SqlSubqueryExpression(
        TypeMapping typeMapping,
        SqlSelectExpression subquery) : base(typeMapping.ClrType, typeMapping)
    {
        Subquery = subquery;
    }

    public SqlSelectExpression Subquery { get; }
}
