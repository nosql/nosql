using System.Reflection;
using NoSql.Query.Expressions;

namespace NoSql.Query.Translators;

public class SqlExpressionTranslatorProvider : ISqlExpressionTranslatorProvider
{
    private readonly IEnumerable<IMethodCallTranslator> _methodTranslators;
    private readonly IEnumerable<IMemberTranslator> _memberTranslators;

    public SqlExpressionTranslatorProvider(
        IEnumerable<IMethodCallTranslator> methodTranslators,
        IEnumerable<IMemberTranslator> memberTranslators)
    {
        _methodTranslators = methodTranslators;
        _memberTranslators = memberTranslators;
    }

    public SqlExpression? TranslateMember(MemberInfo member, SqlExpression? instance)
    {
        foreach (var translator in _memberTranslators)
        {
            var exp = translator.Translate(member, instance);
            if (exp != null)
                return exp;
        }

        return null;
    }

    public SqlExpression? TranslateMethod(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments)
    {
        foreach (var translator in _methodTranslators)
        {
            var exp = translator.Translate(method, instance, arguments);
            if (exp != null)
                return exp;
        }

        return null;
    }
}