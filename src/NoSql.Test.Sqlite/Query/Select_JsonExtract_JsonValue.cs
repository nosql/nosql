namespace NoSql.Test;

[TestClass]
public class Select_JsonExtract_JsonValue
{
    [TestMethod]
    public async Task Object()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.Object);
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
    public async Task Array()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.ArrayInt);
        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Length);
        Assert.AreEqual(0, value[0]);
        Assert.AreEqual(9, value[9]);
    }

}
