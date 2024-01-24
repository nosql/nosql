using NoSql.Query.Translators;
using NoSql.Sqlite.Storage;
using NoSql.Storage;

namespace NoSql.Sqlite.Query.Translators;

public class SqliteStringMemberTranslator : StringMemberTranslator
{
    protected override string LengthFunctionName => "length";
    protected override TypeMapping IntTypeMapping => SqliteTypeMappingSource.Int;
}
