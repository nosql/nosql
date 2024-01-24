namespace NoSql.Test;

[TestClass]
public class Select_Aggregate
{
    [TestMethod]
    public void Count()
    {
        Assert.AreEqual(4, DB.Table<AggregateTestObject>().Count());
    }

    [TestMethod]
    public void Min()
    {
        Assert.AreEqual(1, DB.Table<AggregateTestObject>().Min(x => x.Int));
    }

    [TestMethod]
    public void Max()
    {
        Assert.AreEqual(4, DB.Table<AggregateTestObject>().Max(x => x.Int));
    }

    [TestMethod]
    public void Sum()
    {
        Assert.AreEqual(10, DB.Table<AggregateTestObject>().Sum(x => x.Int));
    }

    [TestMethod]
    public void Avg()
    {
        //List<PrimitiveValueQueryTestObject> a;a.Sum(x => x.Int);
        Assert.AreEqual(2.5, DB.Table<AggregateTestObject>().Average(x => x.Int));
    }

    [TestMethod]
    public void Any()
    {
        Assert.IsTrue(DB.Table<AggregateTestObject>().Where(x => x.Int == 1).Any());
        Assert.IsFalse(DB.Table<AggregateTestObject>().Where(x => x.Int == 0).Any());
    }

    [TestMethod]
    public void All()
    {
        Assert.IsTrue(DB.Table<AggregateTestObject>().Where(x => x.FixedValue == 1).All());
        Assert.IsFalse(DB.Table<AggregateTestObject>().Where(x => x.Int == 1).All());
    }
}
