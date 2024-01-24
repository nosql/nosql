using System.Reflection;
using NoSql.Query.Expressions;

namespace NoSql.Query.Translators;

public interface IMemberTranslatorProvider
{
    SqlExpression? TranslateMember(MemberInfo member, SqlExpression? instance);
}
