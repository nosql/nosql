namespace NoSql.Test;

[TestClass]
public class Method_StringTest
{
    [TestMethod]
    public async Task Length()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.String.Length);
        Assert.AreEqual(PrimitiveValueQueryTestObject.TestStringValue.Length, value);
    }

    [TestMethod]
    public async Task StartsWith()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String.StartsWith("a")).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String.StartsWith("b")).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Contains()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String.Contains("123")).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String.Contains("321")).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task EndsWith()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String.EndsWith("拾")).CountAsync();
        Assert.AreEqual(1, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.String.EndsWith("a")).CountAsync();
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task ToString_Projection()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.Int.ToString());
        Assert.AreEqual(int.MaxValue.ToString(), value);
    }

    [TestMethod]
    public async Task ToString_Where()
    {
        string p = int.MaxValue.ToString();
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Int.ToString() == p).CountAsync();
        Assert.AreEqual(1, value);
    }

}
