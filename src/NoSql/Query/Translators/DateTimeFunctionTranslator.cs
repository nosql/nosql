using System.Reflection;
using NoSql.Query.Expressions;

namespace NoSql.Query.Translators;

public abstract class DateTimeFunctionTranslator : IMemberTranslator
{
    private static readonly PropertyInfo YearPropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Year))!;
    private static readonly PropertyInfo MonthPropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Month))!;
    private static readonly PropertyInfo DayPropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Day))!;
    private static readonly PropertyInfo HourPropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Hour))!;
    private static readonly PropertyInfo MinutePropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Minute))!;
    private static readonly PropertyInfo SecondPropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Second))!;
    private static readonly PropertyInfo MillisecondPropertyInfo = typeof(DateTime).GetRuntimeProperty(nameof(DateTime.Millisecond))!;

    public SqlExpression? Translate(MemberInfo member, SqlExpression? instance)
    {
        if (instance != null)
        {
            if (member == YearPropertyInfo)
                return Translate(DateTimeYear, instance);

            if (member == MonthPropertyInfo)
                return Translate(DateTimeMonth, instance);

            if (member == DayPropertyInfo)
                return Translate(DateTimeDay, instance);

            if (member == HourPropertyInfo)
                return Translate(DateTimeHour, instance);

            if (member == MinutePropertyInfo)
                return Translate(DateTimeMinute, instance);

            if (member == SecondPropertyInfo)
                return Translate(DateTimeSecond, instance);

            if (member == MillisecondPropertyInfo)
                return Translate(DateTimeMillisecond, instance);

        }
        return null;
    }

    protected abstract SqlExpression? Translate(string format, SqlExpression instance);

    public const string DateTimeYear = "year";
    public const string DateTimeMonth = "month";
    public const string DateTimeDay = "day";
    public const string DateTimeHour = "hour";
    public const string DateTimeMinute = "minute";
    public const string DateTimeSecond = "second";
    public const string DateTimeMillisecond = "millisecond";
}