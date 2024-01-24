namespace NoSql.Test;

[TestClass]
public class Where_Cast
{

    [TestMethod]
    public async Task Cast_Value_Integer()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => x.Int == (long)int.MaxValue).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Cast_Column_Integer()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().Where(x => (long)x.Int > 1L).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Cast_JsonColumn_Integer()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => (long)x.Object.Int > 1L).CountAsync();
        Assert.AreEqual(1, value);
    }

}