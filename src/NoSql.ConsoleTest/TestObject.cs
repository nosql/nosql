using Bogus;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.ConsoleTest;

[Table("fake_table")]
public class FakeObject
{
    private static readonly Faker faker = new();

    public bool Bool { get; set; }

    public byte Byte { get; set; }

    public sbyte SByte { get; set; }

    public short Short { get; set; }

    public ushort UShort { get; set; }

    public uint UInt32 { get; set; }

    public int Int32 { get; set; }

    public long Int64 { get; set; }

    //public ulong UInt64 { get; set; }

    public float Float { get; set; }

    public double Double { get; set; }

    public decimal Decimal { get; set; }

    public int? NullableInt { get; set; } = 1;
    public double? NullableDouble { get; set; } = null;
    public char Char { get; set; }

    public string? String { get; set; }

    public string? String2 { get; set; }

    public FakeInt32Enum Enum { get; set; }

    public object NullObject { get; set; } = null!;

    public Guid Guid { get; set; }

    public DateTime DateTime { get; set; }

    public DateTimeOffset DateTimeOffset { get; set; }

    public TimeSpan TimeSpan { get; set; }

    public byte[]? ByteArray { get; set; }

    public string[]? StringArray { get; set; }

    public List<int>? Int32List { get; set; }

    public List<FakeObject>? ObjectArray { get; set; }

    public IEnumerable<string>? Int32Enumerable { get; set; }

    public FakeObject? Object { get; set; }


    public Dictionary<string, object>? Dictionary { get; set; }

    public FakeStruct Struct { get; set; }


    public static IEnumerable<FakeObject> CreateFakeModels(int count = 100)
    {
        return faker.Make(count, () => Create());
    }

    public static FakeObject Create()
    {
        return new FakeObject
        {
            Bool = faker.Random.Bool(),
            Byte = faker.Random.Byte(),
            SByte = faker.Random.SByte(),
            Short = faker.Random.Short(),
            UShort = faker.Random.UShort(),
            Int32 = faker.Random.Int(),
            UInt32 = faker.Random.UInt(),
            Int64 = faker.Random.Long(),
            //UInt64 = faker.Random.ULong(),
            Float = faker.Random.Float(),
            Double = faker.Random.Double(),
            Decimal = faker.Random.Decimal(),
            Char = faker.Random.Char(),
            String = faker.Lorem.Sentence(),
            String2 = faker.Lorem.Paragraph(),
            DateTime = faker.Date.Past(),
            DateTimeOffset = faker.Date.RecentOffset(),
            TimeSpan = DateTime.Now - faker.Date.Past(),
            Enum = faker.PickRandom<FakeInt32Enum>(),
            Guid = Guid.NewGuid(),
            StringArray = faker.Make(5, () => faker.Lorem.Text()).ToArray(),
            Int32List = faker.Make(5, () => faker.Random.Int()).ToList(),
            ByteArray = faker.Random.Bytes(faker.Random.Byte()),
            Int32Enumerable = faker.Make(5, () => faker.Lorem.Text()),
            Struct = new FakeStruct
            {
                String = faker.Address.StreetName(),
                Integer = faker.Random.Int()
            },
            Object = new FakeObject
            {
                Bool = faker.Random.Bool(),
                Byte = faker.Random.Byte(),
                SByte = faker.Random.SByte(),
                Short = faker.Random.Short(),
                UShort = faker.Random.UShort(),
                Int32 = faker.Random.Int(),
                UInt32 = faker.Random.UInt(),
                Int64 = faker.Random.Long(),
                //UInt64 = faker.Random.ULong(),
                Float = faker.Random.Float(),
                Double = faker.Random.Double(),
                Decimal = faker.Random.Decimal(),
                Char = faker.Random.Char(),
                String = faker.Lorem.Sentence(),
                String2 = faker.Lorem.Paragraph(),
                DateTime = faker.Date.Past(),
                DateTimeOffset = faker.Date.RecentOffset(),
                TimeSpan = DateTime.Now - faker.Date.Past(),
                Enum = faker.PickRandom<FakeInt32Enum>(),
                Guid = Guid.NewGuid(),
                StringArray = faker.Make(5, () => faker.Lorem.Text()).ToArray(),
                Int32List = faker.Make(5, () => faker.Random.Int()).ToList(),
                Struct = new FakeStruct
                {
                    String = faker.Address.StreetName(),
                    Integer = faker.Random.Int()
                },
            },
            ObjectArray = faker.Make(5, () => new FakeObject
            {
                Bool = faker.Random.Bool(),
                Byte = faker.Random.Byte(),
                SByte = faker.Random.SByte(),
                Short = faker.Random.Short(),
                UShort = faker.Random.UShort(),
                Int32 = faker.Random.Int(),
                UInt32 = faker.Random.UInt(),
                Int64 = faker.Random.Long(),
                //UInt64 = faker.Random.ULong(),
                Float = faker.Random.Float(),
                Double = faker.Random.Double(),
                Decimal = faker.Random.Decimal(),
                Char = faker.Random.Char(),
                String = faker.Lorem.Sentence(),
                String2 = faker.Lorem.Paragraph(),
                DateTime = faker.Date.Past(),
                DateTimeOffset = faker.Date.RecentOffset(),
                TimeSpan = DateTime.Now - faker.Date.Past(),
                Enum = faker.PickRandom<FakeInt32Enum>(),
                Guid = Guid.NewGuid(),
                StringArray = faker.Make(5, () => faker.Lorem.Text()).ToArray(),
                Int32List = faker.Make(5, () => faker.Random.Int()).ToList(),
                Struct = new FakeStruct
                {
                    String = faker.Address.StreetName(),
                    Integer = faker.Random.Int(),
                },
            }).ToList(),
            Dictionary = new Dictionary<string, object>
            {
                { "bool", faker.Random.Bool() },
                { "byte", faker.Random.Byte() },
                { "sbyte", faker.Random.SByte() },
                { "short", faker.Random.Short() },
                { "ushort", faker.Random.UShort() },
                { "int", faker.Random.Int() },
                { "uint", faker.Random.UInt() },
                { "long", faker.Random.Long() },
                { "ulong", faker.Random.ULong() },
                { "float", faker.Random.Float() },
                { "double", faker.Random.Double() },
                { "decimal", faker.Random.Decimal() },
                { "char", faker.Random.Char() },
                { "string", faker.Lorem.Text() },
                { "datetime", faker.Date.Past() },
                { "datetimeoffset", faker.Date.RecentOffset() },
                { "timespan", DateTime.Now - faker.Date.Past() },
                { "enum", faker.PickRandom<FakeInt32Enum>() },
                { "guid", Guid.NewGuid() },
            }
        };
    }

    public struct FakeStruct
    {
        public string String { get; set; }
        public int Integer { get; set; }
    }

    public enum FakeInt32Enum { A, B, C }

    public enum FakeInt64Enum : long
    {
        A = long.MaxValue,
        B = long.MaxValue - 1,
        C = long.MaxValue - 2,
    }
}
