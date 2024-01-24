using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.SqlServer.Storage;
using NoSql.Storage;

namespace NoSql.SqlServer.Query.Translators;

public class SqlServerStringMemberTranslator : StringMemberTranslator
{
    protected override SqlExpression TranslateLength(SqlExpression instance)
    {
        return new SqlCastExpression(typeof(int), IntTypeMapping, base.TranslateLength(instance));
    }

    protected override string LengthFunctionName => "LEN";

    protected override TypeMapping IntTypeMapping => SqlServerTypeMappingSource.IntTypeMapping;
}