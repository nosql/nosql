using System.Reflection;
using NoSql.Query.Expressions;

namespace NoSql.Query.Translators;

public interface ISqlExpressionTranslatorProvider
{
    SqlExpression? TranslateMember(MemberInfo member, SqlExpression? instance);
    SqlExpression? TranslateMethod(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments);
}
