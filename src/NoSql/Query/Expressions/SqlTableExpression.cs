using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlTableExpression : SqlTableBaseExpression
{
    public SqlTableExpression(Type type, string tableName) 
        : base(type, null, null)
    {
#if !DEBUG
        throw new NotSupportedException();
#endif
        Name = tableName;
        TableInfo = null!;
    }

    public SqlTableExpression(NoSqlTypeInfo tableInfo, string? alias = null)
        : base(tableInfo.ClrType, tableInfo.TypeMapping, alias)
    {
        Name = tableInfo.Name;
        TableInfo = tableInfo;
    }

    public string Name { get; }

    public NoSqlTypeInfo TableInfo { get; }
}
