namespace NoSql.Query.Expressions;

public class SqlColumnValueSetExpression : SqlExpression
{
    public SqlColumnValueSetExpression(SqlColumnExpression column, SqlExpression value) : base(column.Type, column.TypeMapping)
    {
        Column = column;
        Value = value;
    }

    public SqlColumnExpression Column { get; }
    public SqlExpression Value { get; }
}