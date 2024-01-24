using NoSql.Storage;

namespace NoSql.Query.Expressions;

public abstract class SqlTableBaseExpression : SqlExpression
{
    public SqlTableBaseExpression(Type type, TypeMapping? typeMapping, string? alias = null) : base(type, typeMapping)
    {
        Alias = alias;
    }

    public string? Alias { get; }
}
