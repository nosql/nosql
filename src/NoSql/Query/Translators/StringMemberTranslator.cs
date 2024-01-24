using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Reflection;

namespace NoSql.Query.Translators;

public abstract class StringMemberTranslator : IMemberTranslator
{
    private static readonly PropertyInfo LengthPropertyInfo = typeof(string).GetRuntimeProperty(nameof(string.Length))!;

    public SqlExpression? Translate(MemberInfo member, SqlExpression? instance)
    {
        if (member == LengthPropertyInfo && instance != null)
        {
            return TranslateLength(instance);
        }
        return null;
    }

    protected virtual SqlExpression TranslateLength(SqlExpression instance)
    {
        return new SqlFunctionExpression(IntTypeMapping, LengthFunctionName, instance);
    }

    protected abstract string LengthFunctionName { get; }
    protected abstract TypeMapping IntTypeMapping { get; }
}
