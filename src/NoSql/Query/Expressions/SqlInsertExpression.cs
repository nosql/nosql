namespace NoSql.Query.Expressions;

public class SqlInsertExpression : SqlExpression
{
    public SqlInsertExpression(
        SqlTableExpression tableExpression,
        Dictionary<SqlColumnExpression, SqlExpression> values) : base(tableExpression.Type, null)
    {
        Table = tableExpression;
        Columns = values.Keys.ToArray();
        Values = new SqlExpression[][] { values.Values.ToArray() };
    }

    public SqlInsertExpression(
        SqlTableExpression tableExpression,
        SqlColumnExpression[] columns,
        SqlExpression[] values) : base(tableExpression.Type, null)
    {
        Table = tableExpression;
        Columns = columns;
        Values = new SqlExpression[][] { values };
    }

    public SqlInsertExpression(
        SqlTableExpression tableExpression,
        SqlColumnExpression[] columns,
        SqlExpression[][] values) : base(tableExpression.Type, null)
    {
        Table = tableExpression;
        Columns = columns;
        Values = values;
    }

    public SqlTableExpression Table { get; }

    public SqlColumnExpression[] Columns { get; }

    public SqlExpression[][] Values { get; }
}
