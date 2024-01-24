namespace NoSql.Test;

[TestClass]
public class Where_JsonValue
{
    [TestMethod]
    public async Task Bool()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Bool).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => !x.Object.Bool).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Byte()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Byte > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Byte <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task SByte()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.SByte > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.SByte <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Short()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Short > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Short <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task UShort()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.UShort > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.UShort <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Int()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Int > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Int <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task UInt()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.UInt > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.UInt <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Long()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Long > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Long <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task ULong()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.ULong > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.ULong <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Float()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Float > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Float <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Double()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Double > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Double <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Decimal()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Decimal > 0).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Decimal <= 0).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Char()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Char == PrimitiveValueQueryTestObject.TestCharValue).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.Char != PrimitiveValueQueryTestObject.TestCharValue).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task String()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.String == PrimitiveValueQueryTestObject.TestStringValue).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.String != PrimitiveValueQueryTestObject.TestStringValue).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task String2()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.String2 == null).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.String2 != null).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task EnumInt()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.EnumInt32 == Int32Enum.C).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.EnumInt32 != Int32Enum.C).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task EnumLong()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.EnumInt64 == Int64Enum.C).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.EnumInt64 != Int64Enum.C).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Nullable_Null()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.NullableNullValue == null).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.NullableNullValue != null).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Nullable_NotNull()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.NullableNotNull == 1).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.Object.NullableNotNull != 1).CountAsync();
        Assert.AreEqual(0, value);
    }
}
