namespace NoSql.Test;

[TestClass]
public class Select_Cast
{
    [TestMethod]
    public async Task Cast_Integer()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => (long)x.Int);
        Assert.AreEqual((long)int.MaxValue, value);
    }

    [TestMethod]
    public async Task Json_Cast_Integer()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => (long)x.Object.Int);
        Assert.AreEqual((long)int.MaxValue, value);
    }

}
