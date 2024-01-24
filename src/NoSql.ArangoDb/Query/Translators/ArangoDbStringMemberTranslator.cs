using NoSql.ArangoDb.Storage;
using NoSql.Query.Translators;
using NoSql.Storage;

namespace NoSql.ArangoDb.Query.Translators;

public class ArangoDbStringMemberTranslator : StringMemberTranslator
{
    protected override string LengthFunctionName => "LENGTH";
    protected override TypeMapping IntTypeMapping => ArangoDbTypeMappingSource.IntType;
}
