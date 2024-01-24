using NoSql.PostgreSql.Storage;
using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using System.Linq.Expressions;

namespace NoSql.PostgreSql.Query.Translators;

public class PostgreSqlDateTimeFunctionTranslator : DateTimeFunctionTranslator
{
    protected override SqlExpression? Translate(string format, SqlExpression instance)
    {
        switch (format)
        {
            case DateTimeYear: return CreateDatePartFunction("'year'");
            case DateTimeMonth: return CreateDatePartFunction("'month'");
            case DateTimeDay: return CreateDatePartFunction("'day'");
            case DateTimeHour: return CreateDatePartFunction("'hour'");
            case DateTimeMinute: return CreateDatePartFunction("'minute'");
            case DateTimeSecond: return CreateDatePartFunction("'second'");
            case DateTimeMillisecond:
                {
                    var milliseconds = new SqlFunctionExpression(
                        PostgreSqlTypeMappingSource.DoubleTypeMapping,
                        "date_part", 
                        new SqlExpression[] { new SqlFragmentExpression("'milliseconds'"), instance });

                    var floor = new SqlFunctionExpression(
                        PostgreSqlTypeMappingSource.IntTypeMapping,
                        "floor",
                        new SqlFunctionExpression(
                            PostgreSqlTypeMappingSource.DoubleTypeMapping,
                            "date_part",
                            new SqlExpression[] { new SqlFragmentExpression("'second'"), instance }));

                    return new SqlCastExpression(
                        PostgreSqlTypeMappingSource.IntTypeMapping,
                        new SqlBinaryExpression(
                            PostgreSqlTypeMappingSource.IntTypeMapping,
                            ExpressionType.Subtract,
                            milliseconds,
                            new SqlBinaryExpression(
                                PostgreSqlTypeMappingSource.DoubleTypeMapping,
                                ExpressionType.Multiply,
                                floor,
                                new SqlConstantExpression(PostgreSqlTypeMappingSource.IntTypeMapping, 1000))));
                }
            default: throw new NotSupportedException();
        }

        SqlExpression CreateDatePartFunction(string part)
        {
            return new SqlCastExpression(
                PostgreSqlTypeMappingSource.IntTypeMapping,
                new SqlFunctionExpression(
                    PostgreSqlTypeMappingSource.IntTypeMapping,
                    "date_part",
                    new SqlExpression[] { new SqlFragmentExpression(part), instance }));
        }
    }
}
