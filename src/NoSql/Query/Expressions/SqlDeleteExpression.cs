namespace NoSql.Query.Expressions;

public class SqlDeleteExpression : SqlExpression
{
    public SqlDeleteExpression(SqlTableExpression table, SqlExpression? predicate) : base(table.TypeMapping!)
    {
        Table = table;
        Predicate = predicate;
    }

    public SqlTableExpression Table { get; }
    public SqlExpression? Predicate { get; }
}