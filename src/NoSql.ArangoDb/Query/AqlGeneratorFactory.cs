using NoSql.Query;

namespace NoSql.ArangoDb.Query;

public class AqlGeneratorFactory : ISqlGeneratorFactory
{
    public ISqlGenerator Create() => new AqlGenerator();
}
