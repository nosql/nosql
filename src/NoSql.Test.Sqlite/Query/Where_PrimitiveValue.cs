namespace NoSql.Test;


[TestClass]
public class Where_PrimitiveValue
{
    [TestMethod]
    public async Task Bool()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Bool).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => !x.Bool).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Byte()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Byte > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Byte <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task SByte()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.SByte > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.SByte <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Short()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Short > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Short <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task UShort()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.UShort > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.UShort <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Int()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Int > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Int <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task UInt()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.UInt > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.UInt <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Long()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Long > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Long <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task ULong()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.ULong == 0).CountAsync();
        Assert.AreEqual(0, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.ULong != 0).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Float()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Float > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Float <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Double()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Double > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Double <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Decimal()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Decimal > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Decimal <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Char()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Char == PrimitiveValueQueryTestObject.TestCharValue).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Char != PrimitiveValueQueryTestObject.TestCharValue).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task String()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String == PrimitiveValueQueryTestObject.TestStringValue).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String != PrimitiveValueQueryTestObject.TestStringValue).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task String2()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String2 == null).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String2 != null).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task EnumInt()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.EnumInt32 == Int32Enum.C).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.EnumInt32 != Int32Enum.C).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task EnumLong()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.EnumInt64 == Int64Enum.C).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.EnumInt64 != Int64Enum.C).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Nullable_Null()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.NullableNullValue == null).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.NullableNullValue != null).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Nullable_HasValue()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => !x.NullableNullValue.HasValue).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.NullableNullValue.HasValue).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Nullable_NotNull()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.NullableNotNull == 1).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.NullableNotNull != 1).CountAsync();
        Assert.AreEqual(0, value);
    }

}
