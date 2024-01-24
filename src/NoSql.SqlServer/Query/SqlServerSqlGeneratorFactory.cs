using NoSql.Query;

namespace NoSql.SqlServer.Query;

public class SqlServerSqlGeneratorFactory : ISqlGeneratorFactory
{
    public ISqlGenerator Create() => new SqlServerSqlGenerator();
}
