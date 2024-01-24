using NoSql.Test;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Benchmark;

public class BenchmarkPrimitiveValueObject : PrimitiveValueQueryTestObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public static new BenchmarkPrimitiveValueObject Create(int seed)
    {
        return new BenchmarkPrimitiveValueObject
        {
            Bool = true,
            Byte = byte.MaxValue,
            SByte = sbyte.MaxValue,
            Short = short.MaxValue,
            UShort = ushort.MaxValue,
            Int = seed,
            UInt = uint.MaxValue,
            Long = long.MaxValue,
            //UInt64 = ulong.MaxValue,
            Float = float.Pi,
            Double = double.Pi,
            Decimal = Convert.ToDecimal(Math.E),
            Char = TestCharValue,
            String = TestStringValue,
            String2 = null,
            EnumInt32 = Int32Enum.C,
            EnumInt64 = Int64Enum.C,
            DateTime = DateTimeValue
        };
    }
}
