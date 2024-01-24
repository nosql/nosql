namespace NoSql.Test;

[TestClass]
public class Select_PrimitiveValue
{
    [TestMethod]
    public async Task Bool()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Bool);
        Assert.AreEqual(true, value);
    }

    [TestMethod]
    public async Task Byte()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Byte);
        Assert.AreEqual(byte.MaxValue, value);
    }

    [TestMethod]
    public async Task SByte()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.SByte);
        Assert.AreEqual(sbyte.MaxValue, value);
    }

    [TestMethod]
    public async Task Short()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Short);
        Assert.AreEqual(short.MaxValue, value);
    }

    [TestMethod]
    public async Task UShort()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.UShort);
        Assert.AreEqual(ushort.MaxValue, value);
    }

    [TestMethod]
    public async Task Int()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Int);
        Assert.AreEqual(int.MaxValue, value);
    }

    [TestMethod]
    public async Task UInt()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.UInt);
        Assert.AreEqual(uint.MaxValue, value);
    }

    [TestMethod]
    public async Task Long()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Long);
        Assert.AreEqual(long.MaxValue, value);
    }

    [TestMethod]
    public async Task Float()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Float);
        Assert.AreEqual(float.Pi, value);
    }

    [TestMethod]
    public async Task Double()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Double);
        Assert.AreEqual(double.Pi, value);
    }

    [TestMethod]
    public async Task Decimal()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Decimal);
        Assert.AreEqual(2.718281828459040000m, value);
    }

    [TestMethod]
    public async Task Char()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Char);
        Assert.AreEqual('中', value);
    }

    [TestMethod]
    public async Task EnumByte()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.EnumInt8);
        Assert.AreEqual(Int8Enum.C, value);
    }

    [TestMethod]
    public async Task EnumInt()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.EnumInt32);
        Assert.AreEqual(Int32Enum.C, value);
    }

    [TestMethod]
    public async Task EnumLong()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.EnumInt64);
        Assert.AreEqual(Int64Enum.C, value);
    }

    [TestMethod]
    public async Task DateTime()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue, value);
    }


    [TestMethod]
    public async Task String()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.String);
        Assert.AreEqual(PrimitiveValueQueryTestObject.TestStringValue, value);
    }

    [TestMethod]
    public async Task String_Null()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.String2);
        Assert.IsNull(value);
    }

    [TestMethod]
    public async Task Nullable_Null()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.NullableNullValue);
        Assert.IsNull(value);
    }

    [TestMethod]
    public async Task Nullable_NotNull()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.NullableNotNull);
        Assert.AreEqual(1, value);
    }


}