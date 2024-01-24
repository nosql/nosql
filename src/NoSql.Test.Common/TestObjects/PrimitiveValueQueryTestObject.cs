using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Test;

public class PrimitiveValueQueryTestObject
{
    public bool Bool { get; set; }
    public byte Byte { get; set; }
    public sbyte SByte { get; set; }
    public short Short { get; set; }
    public ushort UShort { get; set; }
    public uint UInt { get; set; }
    public int Int { get; set; }
    public int Increment { get; set; } = 1;
    public long Long { get; set; }
    public ulong ULong { get; set; }
    public float Float { get; set; }
    public double Double { get; set; }
    public decimal Decimal { get; set; }
    public char Char { get; set; } = '0';
    public int? NullableNotNull { get; set; } = 1;
    public int? NullableNullValue { get; set; } = null;
    public string? String { get; set; }
    public string? String2 { get; set; }

    public DateTime DateTime { get; set; }

    public Int8Enum EnumInt8 { get; set; }
    public Int32Enum EnumInt32 { get; set; }
    public Int64Enum EnumInt64 { get; set; }

    [NotMapped]
    public int NotMapped { get; set; } = 1;

    public const string TestStringValue = "abcdefg0123456789!@#$\t\r\n，  壹贰叁肆伍陆柒捌玖拾";
    public const char TestCharValue = '中';
    public static readonly DateTime DateTimeValue = new(2001, 1, 1, 1, 1, 1, 111);

    public static PrimitiveValueQueryTestObject Create(int v = int.MaxValue)
    {
        return new PrimitiveValueQueryTestObject
        {
            Bool = true,
            Byte = byte.MaxValue,
            SByte = sbyte.MaxValue,
            Short = short.MaxValue,
            UShort = ushort.MaxValue,
            Int = v,
            UInt = uint.MaxValue,
            Long = long.MaxValue,
            ULong = long.MaxValue,
            Float = float.Pi,
            Double = double.Pi,
            Decimal = Convert.ToDecimal(Math.E),
            Char = TestCharValue,
            String = TestStringValue,
            String2 = null,
            EnumInt8 = Int8Enum.C,
            EnumInt32 = Int32Enum.C,
            EnumInt64 = Int64Enum.C,
            DateTime = DateTimeValue
        };
    }
}
