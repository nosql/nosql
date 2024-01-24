namespace NoSql.Test;

public class PrimitiveValueUpdateTestObject : PrimitiveValueQueryTestObject
{
    public static PrimitiveValueUpdateTestObject Create()
    {
        return new PrimitiveValueUpdateTestObject
        {
            Bool = true,
            Byte = byte.MaxValue,
            SByte = sbyte.MaxValue,
            Short = short.MaxValue,
            UShort = ushort.MaxValue,
            Int = int.MaxValue,
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
