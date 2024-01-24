namespace NoSql.Test;

[TestClass]
public class Method_DateTimeTest
{
    [TestMethod]
    public async Task Year()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Year);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Year, value);
    }

    [TestMethod]
    public async Task Month()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Month);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Month, value);
    }

    [TestMethod]
    public async Task Day()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Day);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Day, value);
    }

    [TestMethod]
    public async Task Hour()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Hour);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Hour, value);
    }

    [TestMethod]
    public async Task Minute()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Minute);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Minute, value);
    }

    [TestMethod]
    public async Task Second()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Second);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Second, value);
    }

    [TestMethod]
    public async Task Millisecond()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => x.DateTime.Millisecond);
        Assert.AreEqual(PrimitiveValueQueryTestObject.DateTimeValue.Millisecond, value);
    }

}
