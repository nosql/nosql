using NoSql.Sqlite.Storage.Mappings;
using NoSql.Storage;
using NoSql.Storage.Mappings;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace NoSql.Sqlite.Storage;

public class SqliteTypeMappingSource : TypeMappingSource
{
    private const string IntegerTypeName = "INTEGER";
    private const string RealTypeName = "REAL";
    private const string BlobTypeName = "BLOB";
    private const string TextTypeName = "TEXT";

    public static readonly LongTypeMapping Long = new(IntegerTypeName);
    public static readonly IntTypeMapping Int = new IntTypeMapping(IntegerTypeName);
    public static readonly DoubleTypeMapping Double = new(RealTypeName);
    private static readonly ByteArrayTypeMapping Blob = new(BlobTypeName);
    private static readonly SqliteStringTypeMapping Text = new(TextTypeName);

    private readonly Dictionary<Type, TypeMapping> _clrTypeMappings = new()
    {
        { typeof(string), Text },
        { typeof(byte[]), Blob },
        { typeof(bool), new BoolTypeMapping(IntegerTypeName) },
        { typeof(byte), new ByteTypeMapping(IntegerTypeName) },
        { typeof(char), new CharTypeMapping(TextTypeName) },
        { typeof(int), Int },
        { typeof(long), Long },
        { typeof(sbyte), new SByteTypeMapping(IntegerTypeName) },
        { typeof(short), new ShortTypeMapping(IntegerTypeName) },
        { typeof(uint), new UIntTypeMapping(IntegerTypeName) },
        { typeof(ulong), new SqliteULongTypeMapping(IntegerTypeName) },
        { typeof(ushort), new UShortTypeMapping(IntegerTypeName) },
        //{ typeof(DateTimeOffset), new SqliteDateTimeOffsetTypeMapping(TextTypeName) },
        //{ typeof(TimeSpan), new TimeSpanTypeMapping(TextTypeName) },
        //{ typeof(DateOnly), new SqliteDateOnlyTypeMapping(TextTypeName) },
        //{ typeof(TimeOnly), new SqliteTimeOnlyTypeMapping(TextTypeName) },
        { typeof(decimal), new SqliteDecimalTypeMapping(RealTypeName) },
        { typeof(double), Double },
        { typeof(float), new FloatTypeMapping(RealTypeName) },
        { typeof(DateTime), new DateTimeTypeMapping(TextTypeName)}
        //{ typeof(Guid), new SqliteGuidTypeMapping(TextTypeName) },
        //{ typeof(JsonElement), new SqliteJsonTypeMapping(TextTypeName) }
    };

    private readonly JsonSerializerOptions _serializerOptions;

    public SqliteTypeMappingSource(NoSqlOptions options)
    {
        _serializerOptions = options.SerializerOptions;
    }

    protected override bool TryGetMapping(Type type, [NotNullWhen(true)] out TypeMapping? mapping)
    {
        return _clrTypeMappings.TryGetValue(type, out mapping);
    }

    public override JsonTypeMapping GetJsonTypeMapping(Type type)
    {
        return new JsonTypeMapping(type, TextTypeName, _serializerOptions);
    }
}
