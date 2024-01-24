using NoSql.Extensions;
using NoSql.Storage;
using System.Data.Common;
using System.Text.Json;

namespace NoSql.ArangoDb.Storage;

internal class ArangoDbTypeMappingSource : ISqlTypeMappingSource
{
    public static readonly ArangoDbTypeMapping BoolType = new BooleanTypeMapping();
    public static readonly ArangoDbTypeMapping IntType = new NumberTypeMapping(typeof(int));
    public static readonly ArangoDbTypeMapping StringType = new StringTypeMapping();
    public static readonly ArangoDbTypeMapping DateTimeType = new DateTimeTypeMapping();
    public static readonly ArangoDbTypeMapping DoubleType = new NumberTypeMapping(typeof(double));

    private static readonly Dictionary<Type, ArangoDbTypeMapping> _clrTypeMappings = new()
    {
        { typeof(string),   StringType },
        { typeof(byte[]),   new BooleanTypeMapping() },
        { typeof(bool),     BoolType },
        { typeof(byte),     new NumberTypeMapping(typeof(byte)) },
        { typeof(char),     StringType },
        { typeof(int),      IntType },
        { typeof(long),     new NumberTypeMapping(typeof(long)) },
        { typeof(sbyte),    new NumberTypeMapping(typeof(sbyte)) },
        { typeof(short),    new NumberTypeMapping(typeof(short)) },
        { typeof(uint),     new NumberTypeMapping(typeof(uint)) },
        { typeof(ulong),    new NumberTypeMapping(typeof(ulong)) },
        { typeof(ushort),   new NumberTypeMapping(typeof(ushort)) },
        { typeof(decimal),  new NumberTypeMapping(typeof(decimal)) },
        { typeof(double),   DoubleType },
        { typeof(float),    new NumberTypeMapping(typeof(float)) },
        { typeof(DateTime), DateTimeType },
    };

    public TypeMapping FindMapping(Type type, bool jsonType = false)
    {
        if (jsonType)
        {
            return new JsonTypeMapping(type);
        }

        var underingType = type.UnwrapNullableType();

        if (underingType.IsEnum)
        {
            underingType = underingType.GetEnumUnderlyingType();
        }

        if (_clrTypeMappings.TryGetValue(underingType, out var mapping))
        {
            return mapping;
        }

        return new JsonTypeMapping(type);
    }

    public class BooleanTypeMapping : ArangoDbTypeMapping
    {
        public BooleanTypeMapping() : base(typeof(bool)) { }

        protected override string GenerateNonNullSqlLiteral(object value) => ((bool)value!) ? "true" : "false";
        public override bool IsJsonType => false;
    }

    public class NumberTypeMapping : ArangoDbTypeMapping
    {
        public NumberTypeMapping(Type clrType) : base(clrType) { }
        public override bool IsJsonType => false;
    }

    public class StringTypeMapping : ArangoDbTypeMapping
    {
        public StringTypeMapping() : base(typeof(string)) { }

        protected override string GenerateNonNullSqlLiteral(object value) => $"'{value}'";
        public override bool IsJsonType => false;
    }

    public class DateTimeTypeMapping : ArangoDbTypeMapping
    {
        public DateTimeTypeMapping() : base(typeof(DateTime)) { }

        protected override string GenerateNonNullSqlLiteral(object value) => $"'{((DateTime)value):yyyy-MM-ddTHH:mm:sss.fff}'";
        public override bool IsJsonType => false;
    }

    public class JsonTypeMapping : ArangoDbTypeMapping
    {
        private readonly JsonSerializerOptions? _options;

        public JsonTypeMapping(Type clrType, JsonSerializerOptions? options = null) : base(clrType)
        {
            _options = options;
        }

        public override bool IsJsonType => true;

        protected override string GenerateNonNullSqlLiteral(object value) => JsonSerializer.Serialize(value, _options);

    }

    public class ArangoDbTypeMapping : TypeMapping
    {
        public ArangoDbTypeMapping(Type clrType) : base(clrType, null!) { }

        public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GenerateSqlLiteral(object? value)
        {
            value = NormalizeEnumValue(value);
            return value == null ? "null" : GenerateNonNullSqlLiteral(value);
        }
    }

}


