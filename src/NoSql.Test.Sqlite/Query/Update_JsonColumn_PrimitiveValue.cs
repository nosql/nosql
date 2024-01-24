namespace NoSql.Test;

[TestClass]
public class Update_JsonColumn_PrimitiveValue
{
    [TestMethod]
    public async Task Increment()
    {
        await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Increment, 1));
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Increment, x => x.Object.Increment + 1));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Increment);
        Assert.AreEqual(2, v);
    }

    [TestMethod]
    public async Task Bool()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Bool, x => false));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Bool);
        Assert.AreEqual(false, e);
    }

    [TestMethod]
    public async Task Byte()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Byte, x => 1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Byte);
        Assert.AreEqual((byte)1, e);
    }

    [TestMethod]
    public async Task Short()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Short, x => 1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Short);
        Assert.AreEqual((short)1, e);
    }

    [TestMethod]
    public async Task Int()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Int, x => 1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Int);
        Assert.AreEqual(1, e);
    }

    [TestMethod]
    public async Task Long()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Long, x => 1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Int);
        Assert.AreEqual(1L, e);
    }

    [TestMethod]
    public async Task SByte()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.SByte, x => 1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.SByte);
        Assert.AreEqual((sbyte)1, e);
    }

    [TestMethod]
    public async Task UShort()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.UShort, x => 1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.UShort);
        Assert.AreEqual((ushort)1, e);
    }

    [TestMethod]
    public async Task UInt()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.UInt, x => 1u));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.UInt);
        Assert.AreEqual((uint)1, e);
    }

    [TestMethod]
    public async Task ULong()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.ULong, x => 1ul));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.ULong);
        Assert.AreEqual(1ul, e);
    }

    [TestMethod]
    public async Task Float()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Float, x => float.E));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Float);
        Assert.AreEqual(float.E, e);
    }

    [TestMethod]
    public async Task Double()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Double, x => 1.1));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Double);
        Assert.AreEqual(1.1, e);
    }

    [TestMethod]
    public async Task Decimal()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Decimal, x => 1.1m));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Decimal);
        Assert.AreEqual(1.1m, e);
    }

    [TestMethod]
    public async Task Char()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Char, x => 'a'));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.Char);
        Assert.AreEqual('a', e);
    }

    [TestMethod]
    public async Task Enum_Int32()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.EnumInt32, x => Int32Enum.B));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.EnumInt32);
        Assert.AreEqual(Int32Enum.B, e);
    }

    [TestMethod]
    public async Task Enum_Int64()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.EnumInt64, x => Int64Enum.B));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.EnumInt64);
        Assert.AreEqual(Int64Enum.B, e);
    }

    [TestMethod]
    public async Task Nullable_Null()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.NullableNotNull, x => null));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.NullableNotNull);
        Assert.AreEqual(null, e);
    }

    [TestMethod]
    public async Task String()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.String, x => "aa"));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.String);
        Assert.AreEqual("aa", e);
    }

    [TestMethod]
    public async Task String_Null()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.String2, x => null));
        Assert.AreEqual(1, r);

        var e = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.Object.String2);
        Assert.AreEqual(null, e);
    }



}