using NoSql.ArangoDb.Storage;
using NoSql.Query.Expressions;
using NoSql.Query.Translators;

namespace NoSql.ArangoDb.Query.Translators;

public sealed class ArangoDbDateTimeFunctionTranslator : DateTimeFunctionTranslator
{
    protected override SqlExpression? Translate(string format, SqlExpression instance)
    {
        switch (format)
        {
            case DateTimeYear: return CreateStrftimeFunction("DATE_YEAR");
            case DateTimeMonth: return CreateStrftimeFunction("DATE_MONTH");
            case DateTimeDay: return CreateStrftimeFunction("DATE_DAY");
            case DateTimeHour: return CreateStrftimeFunction("DATE_HOUR");
            case DateTimeMinute: return CreateStrftimeFunction("DATE_MINUTE");
            case DateTimeSecond: return CreateStrftimeFunction("DATE_SECOND");
            case DateTimeMillisecond: return CreateStrftimeFunction("DATE_MILLISECOND");
            default: throw new NotSupportedException();
        }

        SqlExpression CreateStrftimeFunction(string function)
        {
            return new SqlFunctionExpression(ArangoDbTypeMappingSource.IntType, function, new SqlExpression[] { instance });
        }
    }
}
