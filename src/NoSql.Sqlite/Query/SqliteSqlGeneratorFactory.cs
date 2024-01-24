using NoSql.Query;

namespace NoSql.Sqlite.Query;

public class SqliteSqlGeneratorFactory : ISqlGeneratorFactory
{
    public ISqlGenerator Create() => new SqliteSqlGenerator();
}
