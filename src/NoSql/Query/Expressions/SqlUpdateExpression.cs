namespace NoSql.Query.Expressions;

public class SqlUpdateExpression : SqlExpression
{
    public SqlUpdateExpression(
        IList<SqlColumnValueSetExpression> setters,
        SqlTableExpression tableExpression,
        SqlExpression? predicate) : base(tableExpression.Type, null)
    {
        Setters = setters;
        Table = tableExpression;
        Predicate = predicate;
    }

    public IList<SqlColumnValueSetExpression> Setters { get; }

    public SqlTableExpression Table { get; }

    public SqlExpression? Predicate { get; }

    public IList<SqlColumnValueSetExpression> GetSettersWithJsonSetNest() => SqlExpressionHelper.ConsolidateWithJsonSetNest(Setters);
}
