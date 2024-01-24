using NoSql.Query.Expressions;

namespace NoSql.Query;

public interface ISqlGenerator
{
    string Generate(SqlExpression expression);
}