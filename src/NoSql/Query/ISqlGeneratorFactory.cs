using NoSql.Query;

namespace NoSql;

public interface ISqlGeneratorFactory
{
    ISqlGenerator Create();
}
