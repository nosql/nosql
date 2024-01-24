namespace NoSql.Test;

[TestClass]
public class Select_LimitOffset
{
    [TestMethod]
    public void LimitOffset()
    {
        Assert.AreEqual(3, DB.Table<AggregateTestObject>().Skip(1).Take(5).FindAll().Count());
        Assert.AreEqual(3, DB.Table<AggregateTestObject>().Limit(1, 5).FindAll().Count());
    }

    [TestMethod]
    public void Limit()
    {
        Assert.AreEqual(2, DB.Table<AggregateTestObject>().Take(2).FindAll().Count());
    }

    [TestMethod]
    public void Offset()
    {
        Assert.AreEqual(3, DB.Table<AggregateTestObject>().Skip(1).FindAll().Count());
    }
}
