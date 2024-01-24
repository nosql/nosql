using NoSql.PostgreSql.Storage.Mappings;
using NoSql.Storage;
using NoSql.Storage.Mappings;
using System.Diagnostics.CodeAnalysis;

namespace NoSql.PostgreSql.Storage;

public class PostgreSqlTypeMappingSource : TypeMappingSource
{
    public static readonly TypeMapping BoolTypeMapping = new PostgreSqlBoolTypeMapping();
    public static readonly TypeMapping IntTypeMapping = new IntTypeMapping("integer");
    public static readonly TypeMapping DoubleTypeMapping = new DoubleTypeMapping("double precision");
    public static readonly TypeMapping StringTypeMapping = new StringTypeMapping("text");

    private readonly Dictionary<Type, TypeMapping> _clrTypeMappings = new()
    {
        { typeof(string), StringTypeMapping },
        { typeof(byte[]), new PostgreSqlByteArrayTypeMapping() },
        { typeof(bool), BoolTypeMapping },
        { typeof(byte), new ByteTypeMapping("smallint") },
        { typeof(char), new CharTypeMapping("character(1)") },
        { typeof(int), IntTypeMapping },
        { typeof(long), new LongTypeMapping("bigint") },
        { typeof(sbyte), new SByteTypeMapping("smallint") },
        { typeof(short), new ShortTypeMapping("smallint") },
        { typeof(uint), new UIntTypeMapping("bigint") },
        { typeof(ulong), new ULongTypeMapping("bigint") },
        { typeof(ushort), new UShortTypeMapping("integer") },
        //{ typeof(DateTime), new SqliteDateTimeTypeMapping(TextTypeName) },
        //{ typeof(DateTimeOffset), new SqliteDateTimeOffsetTypeMapping(TextTypeName) },
        //{ typeof(TimeSpan), new TimeSpanTypeMapping(TextTypeName) },
        //{ typeof(DateOnly), new SqliteDateOnlyTypeMapping(TextTypeName) },
        //{ typeof(TimeOnly), new SqliteTimeOnlyTypeMapping(TextTypeName) },
        { typeof(decimal), new DecimalTypeMapping("numeric") },
        { typeof(double), DoubleTypeMapping },
        { typeof(float), new FloatTypeMapping("real") },
        { typeof(DateTime), new DateTimeTypeMapping("timestamp") }
        //{ typeof(Guid), new SqliteGuidTypeMapping(TextTypeName) },
        //{ typeof(JsonElement), new SqliteJsonTypeMapping(TextTypeName) }
    };

    private readonly NoSqlOptions _options;

    public PostgreSqlTypeMappingSource(NoSqlOptions options)
    {
        _options = options;
    }

    public override TypeMapping FindMapping(Type type, bool jsonType = false)
    {
        if (jsonType)
        {
            return GetJsonTypeMapping(type);
        }

        return base.FindMapping(type, jsonType);
    }

    public override JsonTypeMapping GetJsonTypeMapping(Type type)
    {
        return new JsonTypeMapping(type, "jsonb", _options.SerializerOptions);
    }

    protected override bool TryGetMapping(Type type, [NotNullWhen(true)] out TypeMapping? mapping)
    {
        return _clrTypeMappings.TryGetValue(type, out mapping);
    }
}
