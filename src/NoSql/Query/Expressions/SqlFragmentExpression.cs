namespace NoSql.Query.Expressions;

public class SqlFragmentExpression : SqlExpression
{
    public SqlFragmentExpression(string sql) : base(typeof(object), null)
    {
        Sql = sql;
    }

    public string Sql { get; }
}