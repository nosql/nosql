using System.Reflection;
using NoSql.Query.Expressions;

namespace NoSql.Query.Translators;

public interface IMethodCallTranslator
{
    SqlExpression? Translate(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments);
}
