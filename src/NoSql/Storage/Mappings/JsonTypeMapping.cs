using System.Data.Common;
using System.Text.Json;

namespace NoSql.Storage.Mappings;

public class JsonTypeMapping : TypeMapping
{
    private readonly JsonSerializerOptions? _options;

    public JsonTypeMapping(Type clrType, string storeType, JsonSerializerOptions? options = null) : base(clrType, storeType)
    {
        _options = options;
    }
    public override bool IsJsonType => true;
    protected override string GenerateNonNullSqlLiteral(object value) => $"'{EscapeSqlLiteral(JsonSerializer.Serialize(value, _options))}'";
    protected virtual string EscapeSqlLiteral(string literal) => literal.Replace("'", "''");

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => JsonSerializer.Deserialize(reader.GetString(ordinal), ClrType, _options)!;
}