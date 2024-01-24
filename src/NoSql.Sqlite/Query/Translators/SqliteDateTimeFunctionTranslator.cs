using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.Sqlite.Storage;

namespace NoSql.Sqlite.Query.Translators;

public class SqliteDateTimeFunctionTranslator : DateTimeFunctionTranslator
{
    protected override SqlExpression? Translate(string format, SqlExpression instance)
    {
        switch (format)
        {
            case DateTimeYear: return CreateStrftimeFunction("'%Y'");
            case DateTimeMonth: return CreateStrftimeFunction("'%M'");
            case DateTimeDay: return CreateStrftimeFunction("'%d'");
            case DateTimeHour: return CreateStrftimeFunction("'%H'");
            case DateTimeMinute: return CreateStrftimeFunction("'%m'");
            case DateTimeSecond: return CreateStrftimeFunction("'%S'");
            case DateTimeMillisecond:
                {
                    return new SqlCastExpression(SqliteTypeMappingSource.Int,
                            new SqlBinaryExpression(
                                SqliteTypeMappingSource.Double,
                                System.Linq.Expressions.ExpressionType.Subtract,
                                CreateMultiply(new SqlFunctionExpression(SqliteTypeMappingSource.Double, "strftime", new SqlExpression[] { new SqlFragmentExpression("'%f'"), instance, })),
                                CreateMultiply(new SqlFunctionExpression(SqliteTypeMappingSource.Double, "floor", new SqlFunctionExpression(SqliteTypeMappingSource.Double, "strftime", new SqlExpression[] { new SqlFragmentExpression("'%f'"), instance, })))));
                }
            default: throw new NotSupportedException();
        }

        SqlExpression CreateStrftimeFunction(string format)
        {
            return new SqlFunctionExpression(SqliteTypeMappingSource.Int, "strftime", new SqlExpression[] { new SqlFragmentExpression(format), instance });
        }

        SqlExpression CreateMultiply(SqlExpression expression)
        {
            return new SqlBinaryExpression(
                SqliteTypeMappingSource.Double,
                System.Linq.Expressions.ExpressionType.Multiply,
                expression,
                new SqlConstantExpression(SqliteTypeMappingSource.Int, 1000));
        }

    }
}
