using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlInExpression : SqlExpression
{
    public SqlInExpression(TypeMapping typeMapping, SqlExpression item, SqlExpression values, bool isNegated = false)
        : base(typeMapping)
    {
        Item = item;
        Values = values;
        IsNegated = isNegated;
    }

    public bool IsNegated { get; }
    public SqlExpression Item { get; }
    public SqlExpression Values { get; }
}