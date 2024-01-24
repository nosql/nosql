using NoSql.Storage;
using NoSql.Storage.Mappings;
using System.Diagnostics.CodeAnalysis;

namespace NoSql.SqlServer.Storage;

public class SqlServerTypeMappingSource : TypeMappingSource
{
    public static readonly TypeMapping BoolTypeMapping = new BoolTypeMapping("bit");
    public static readonly TypeMapping IntTypeMapping = new IntTypeMapping("int");
    public static readonly TypeMapping DoubleTypeMapping = new DoubleTypeMapping("float");

    public static readonly SqlServerTypeMappingSource Default = new();

    private readonly Dictionary<Type, TypeMapping> _clrTypeMappings = new()
    {
        { typeof(string), new StringTypeMapping("nvarchar(max)") },
        { typeof(byte[]), new SqlServerByteArrayTypeMapping() },
        { typeof(bool), BoolTypeMapping },
        { typeof(byte), new ByteTypeMapping("tinyint") },
        { typeof(char), new CharTypeMapping("nchar") },
        { typeof(int), IntTypeMapping },
        { typeof(long), new LongTypeMapping("bigint") },
        { typeof(sbyte), new SByteTypeMapping("smallint") },
        { typeof(short), new ShortTypeMapping("smallint") },
        { typeof(uint), new UIntTypeMapping("bigint") },
        { typeof(ulong), new ULongTypeMapping("bigint") },
        { typeof(ushort), new UShortTypeMapping("int") },
        //{ typeof(DateTime), new SqliteDateTimeTypeMapping(TextTypeName) },
        //{ typeof(DateTimeOffset), new SqliteDateTimeOffsetTypeMapping(TextTypeName) },
        //{ typeof(TimeSpan), new TimeSpanTypeMapping(TextTypeName) },
        //{ typeof(DateOnly), new SqliteDateOnlyTypeMapping(TextTypeName) },
        //{ typeof(TimeOnly), new SqliteTimeOnlyTypeMapping(TextTypeName) },
        { typeof(decimal), new DecimalTypeMapping("decimal(30, 18)") },
        { typeof(double), DoubleTypeMapping },
        { typeof(float), new FloatTypeMapping("real") },
        { typeof(DateTime), new DateTimeTypeMapping("datetime2(3)") }
        //{ typeof(Guid), new SqliteGuidTypeMapping(TextTypeName) },
        //{ typeof(JsonElement), new SqliteJsonTypeMapping(TextTypeName) }
    };

    public override JsonTypeMapping GetJsonTypeMapping(Type type)
    {
        return new JsonTypeMapping(type, "nvarchar(max)");
    }

    protected override bool TryGetMapping(Type type, [NotNullWhen(true)] out TypeMapping? mapping)
    {
        return _clrTypeMappings.TryGetValue(type, out mapping);
    }
}
