namespace NoSql.Query.Expressions;

public class SqlJsonBuildArrayExpression : SqlExpression
{
    public SqlJsonBuildArrayExpression(SqlSelectExpression subquery) : base(subquery.TypeMapping!)
    {
        Subquery = subquery;
    }

    public SqlSelectExpression Subquery { get; }

}