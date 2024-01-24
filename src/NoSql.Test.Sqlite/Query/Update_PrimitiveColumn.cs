namespace NoSql.Test;

[TestClass]
public class Update_ValueColumn
{
    [TestMethod]
    public async Task Increment()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Increment, x => x.Increment + 1));
        Assert.AreEqual(1, r);

        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Increment);
        Assert.AreEqual(2, v);
    }

    [TestMethod]
    public async Task Bool()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Bool, x => false));
        Assert.AreEqual(1, r);

        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Bool);
        Assert.AreEqual(false, v);
    }

    [TestMethod]
    public async Task Byte()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Byte, x => 1));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Byte);
        Assert.AreEqual((byte)1, v);
    }

    [TestMethod]
    public async Task Short()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Short, x => 1));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Short);
        Assert.AreEqual((short)1, v);
    }

    [TestMethod]
    public async Task Int()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Int, x => 1));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Int);
        Assert.AreEqual(1, v);
    }

    [TestMethod]
    public async Task Long()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Long, x => 1));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Long);
        Assert.AreEqual(1L, v);
    }

    [TestMethod]
    public async Task SByte()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.SByte, x => 1));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.SByte);
        Assert.AreEqual((sbyte)1, v);
    }


    [TestMethod]
    public async Task UShort()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.UShort, x => 1));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.UShort);
        Assert.AreEqual((ushort)1, v);
    }

    [TestMethod]
    public async Task UInt()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.UInt, x => 1u));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.UInt);
        Assert.AreEqual((uint)1, v);
    }

    [TestMethod]
    public async Task ULong()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.ULong, x => 1ul));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.ULong);
        Assert.AreEqual((ulong)1, v);
    }

    [TestMethod]
    public async Task Float()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Float, x => float.E));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Float);
        Assert.AreEqual(float.E, v);
    }

    [TestMethod]
    public async Task Double()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Double, x => double.E));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Double);
        Assert.AreEqual(double.E, v);
    }

    [TestMethod]
    public async Task Decimal()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Decimal, x => 1.1m));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Decimal);
        Assert.AreEqual(1.1m, v);
    }

    [TestMethod]
    public async Task Char()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Char, x => 'a'));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.Char);
        Assert.AreEqual('a', v);
    }

    [TestMethod]
    public async Task String()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.String, x => "aa"));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.String);
        Assert.AreEqual("aa", v);
    }

    [TestMethod]
    public async Task String_Null()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.String2, x => null));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.String2);
        Assert.AreEqual(null, v);
    }

    [TestMethod]
    public async Task Nullable_Null()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.NullableNotNull, x => null));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.NullableNotNull);
        Assert.AreEqual(null, v);
    }

    [TestMethod]
    public async Task Enum_Int32()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.EnumInt32, x => Int32Enum.B));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.EnumInt32);
        Assert.AreEqual(Int32Enum.B, v);
    }

    [TestMethod]
    public async Task Enum_Int64()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.EnumInt64, x => Int64Enum.B));
        Assert.AreEqual(1, r);
        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => x.EnumInt64);
        Assert.AreEqual(Int64Enum.B, v);
    }

}
