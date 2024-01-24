using NoSql.Query;

namespace NoSql.PostgreSql.Query;

public class PostgreSqlGeneratorFactory : ISqlGeneratorFactory
{
    public ISqlGenerator Create() => new PostgreSqlGenerator();
}
