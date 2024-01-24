namespace NoSql.Test;

[TestClass]
public class Where_NoExpression
{
    [TestMethod]
    public async Task Where()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().WhereEq("Byte", 0).CountAsync();
        Assert.AreEqual(0, value);
        value = await DB.Table<PrimitiveValueQueryTestObject>().WhereEq("Byte", byte.MaxValue).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Where_Json()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().WhereEq("Object.Byte", 0).CountAsync();
        Assert.AreEqual(0, value);
        value = await DB.Table<JsonValueQueryTestObject>().WhereEq("Object.Byte", byte.MaxValue).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task OrderBy()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().OrderBy("Byte").FindAllAsync();
    }

    [TestMethod]
    public async Task OrderBy_Json()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().OrderBy("Object.Byte").FindAllAsync();
    }
}