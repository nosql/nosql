using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.SqlServer.Storage;

namespace NoSql.SqlServer.Query.Translators;

public class SqlServerDateTimeMemberTranslator : DateTimeFunctionTranslator
{
    protected override SqlExpression? Translate(string format, SqlExpression instance)
    {
        return format switch
        {
            DateTimeYear => CreateDatePartFunction("year"),
            DateTimeMonth => CreateDatePartFunction("month"),
            DateTimeDay => CreateDatePartFunction("day"),
            DateTimeHour => CreateDatePartFunction("hour"),
            DateTimeMinute => CreateDatePartFunction("minute"),
            DateTimeSecond => CreateDatePartFunction("second"),
            DateTimeMillisecond => CreateDatePartFunction("millisecond"),
            _ => throw new NotSupportedException(),
        };
        SqlExpression CreateDatePartFunction(string part)
        {
            return new SqlFunctionExpression(SqlServerTypeMappingSource.IntTypeMapping, "DATEPART", new SqlExpression[] { new SqlFragmentExpression(part), instance });
        }
    }
}
