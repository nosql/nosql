using System.Reflection;
using NoSql.Query.Expressions;

namespace NoSql.Query.Translators;

public interface IMemberTranslator
{
    SqlExpression? Translate(MemberInfo member, SqlExpression? instance);
}
