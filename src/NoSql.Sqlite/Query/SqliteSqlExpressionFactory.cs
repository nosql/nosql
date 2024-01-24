using NoSql.Query;
using NoSql.Storage;

namespace NoSql.Sqlite.Query;

public class SqliteSqlExpressionFactory : SqlExpressionFactory
{
    public SqliteSqlExpressionFactory(ISqlTypeMappingSource typeMappingSource) : base(typeMappingSource)
    {
    }

    protected override bool CanConvertJsonMerge => true;
}
