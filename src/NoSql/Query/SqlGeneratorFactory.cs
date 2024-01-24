using NoSql.Query;

namespace NoSql;

internal class SqlGeneratorFactory : ISqlGeneratorFactory
{
    public ISqlGenerator Create() => new SqlGenerator();
}