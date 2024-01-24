using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Test;

[TestClass]
public class Select_ObjectValue
{
    [TestMethod]
    public async Task QueryOne_SelectAnonymousType()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new
        {
            x.Byte,
            x.SByte,
            x.Short,
            x.UShort,
            x.Int,
            x.UInt,
            x.Long,
            x.Double,
            x.Float,
            x.Bool,
            x.Char,
            x.EnumInt64,
            x.EnumInt32,
            x.String,
            x.String2,
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(value.Byte, byte.MaxValue);
        Assert.AreEqual(value.SByte, sbyte.MaxValue);
        Assert.AreEqual(value.Short, short.MaxValue);
        Assert.AreEqual(value.UShort, ushort.MaxValue);
        Assert.AreEqual(value.Int, int.MaxValue);
        Assert.AreEqual(value.UInt, uint.MaxValue);
        Assert.AreEqual(value.Long, long.MaxValue);

        Assert.AreEqual(value.Float, float.Pi);
        Assert.AreEqual(value.Double, double.Pi);
        Assert.AreEqual(value.Bool, true);
        Assert.AreEqual(value.Char, '中');
        Assert.AreEqual(PrimitiveValueQueryTestObject.TestStringValue, value.String);
        Assert.AreEqual(value.String2, null);
    }


    [TestMethod]
    public async Task QueryOne_SelectAll_DefaultObject()
    {
        var value = await DB.Table<object>(nameof(PrimitiveValueQueryTestObject)).FindAllAsync();
    }

    [TestMethod]
    public async Task QueryOne_SelectOne_DefaultType()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync();
        Assert.IsNotNull(value);
        Assert.AreEqual(byte.MaxValue, value.Byte);
        Assert.AreEqual(sbyte.MaxValue, value.SByte);
        Assert.AreEqual(short.MaxValue, value.Short);
        Assert.AreEqual(ushort.MaxValue, value.UShort);
        Assert.AreEqual(int.MaxValue, value.Int);
        Assert.AreEqual(uint.MaxValue, value.UInt);
        Assert.AreEqual(long.MaxValue, value.Long);

        Assert.AreEqual(float.Pi, value.Float);
        Assert.AreEqual(double.Pi, value.Double);
        Assert.AreEqual(true, value.Bool);
        Assert.AreEqual('中', value.Char);
        Assert.AreEqual(PrimitiveValueQueryTestObject.TestStringValue, value.String);
        Assert.AreEqual(null, value.String2);
    }

    [TestMethod]
    public async Task QueryOne_SelectTableType()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x);
        Assert.IsNotNull(value);
        Assert.AreEqual(byte.MaxValue, value.Byte);
        Assert.AreEqual(sbyte.MaxValue, value.SByte);
        Assert.AreEqual(short.MaxValue, value.Short);
        Assert.AreEqual(ushort.MaxValue, value.UShort);
        Assert.AreEqual(int.MaxValue, value.Int);
        Assert.AreEqual(uint.MaxValue, value.UInt);
        Assert.AreEqual(long.MaxValue, value.Long);

        Assert.AreEqual(float.Pi, value.Float);
        Assert.AreEqual(double.Pi, value.Double);
        Assert.AreEqual(true, value.Bool);
        Assert.AreEqual('中', value.Char);
        Assert.AreEqual(PrimitiveValueQueryTestObject.TestStringValue, value.String);
        Assert.AreEqual(null, value.String2);
    }

    [TestMethod]
    public async Task QueryOne_SelectNewType_AllProperties()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new T1
        {
            F = x.EnumInt64,
            E = x.EnumInt32,
            A = x.Bool,
            B = x.Int,
            C = x.String,
            D = x.EnumInt8,
        });
        Assert.IsNotNull(value);
        Assert.AreEqual(true, value.A);
        Assert.AreEqual(int.MaxValue, value.B);
        Assert.AreEqual(PrimitiveValueQueryTestObject.TestStringValue, value.C);
        Assert.AreEqual(Int8Enum.C, value.D);
        Assert.AreEqual(Int32Enum.C, value.E);
        Assert.AreEqual(Int64Enum.C, value.F);
    }

    [TestMethod]
    public async Task QueryOne_SelectNewType_WithNotMappedColumn()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new T2
        {
            Value = x.Bool,
            NotMapped = 3,
        });
        Assert.IsNotNull(value);
        Assert.AreEqual(true, value.Value);
        Assert.AreEqual(3, value.NotMapped);
    }


    [TestMethod]
    public async Task QueryOne_SelectNewType()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new T1
        {
            F = x.EnumInt64,
            E = x.EnumInt32,
            A = x.Bool,
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(true, value.A);
        Assert.AreEqual(Int32Enum.C, value.E);
        Assert.AreEqual(Int64Enum.C, value.F);

        Assert.AreEqual(0, value.B);
        Assert.AreEqual(null, value.C);
        Assert.AreEqual(Int8Enum.A, value.D);
    }

    [TestMethod]
    public async Task QueryOne_SelectNewType_WithConstants()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new T1
        {
            A = x.Bool,
            B = 2,
        });

        Assert.IsNotNull(value);

        Assert.AreEqual(true, value.A);
        Assert.AreEqual(2, value.B);

        Assert.AreEqual(null, value.C);
        Assert.AreEqual(Int8Enum.A, value.D);
        Assert.AreEqual(Int32Enum.B, value.E);
        Assert.AreEqual(Int64Enum.B, value.F);
    }

    [TestMethod]
    public async Task QueryOne_SelectNewType_WithSingleValue()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new T1
        {
            A = x.Bool,
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(true, value.A);
    }

    [TestMethod]
    public async Task QueryOne_SelectNewType_WithNullableTypeConvert()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => new T2
        {
            Value = x.Bool,
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(true, value.Value);
    }



    public class T1
    {
        public bool A { get; set; }
        public int B { get; set; }
        public string? C { get; set; }
        public Int8Enum D { get; set; }
        public Int32Enum E { get; set; }
        public Int64Enum F { get; set; }


    }

    public class T2
    {
        public bool? Value { get; set; }
        [NotMapped]
        public int NotMapped { get; set; }
    }
}
