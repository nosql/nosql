using NoSql.PostgreSql.Storage;
using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.Storage;
using NoSql.Storage.Mappings;
using System.Data.Common;

namespace NoSql.PostgreSql.Query.Translators;

public class PostgreSqlStringMemberTranslator : StringMemberTranslator
{
    protected override SqlExpression TranslateLength(SqlExpression instance)
    {
        return new SqlCastExpression(typeof(int), PostgreSqlTypeMappingSource.IntTypeMapping, base.TranslateLength(instance));
    }

    protected override string LengthFunctionName => "length";

    protected override TypeMapping IntTypeMapping => PostgreSqlTypeMappingSource.IntTypeMapping;
}

