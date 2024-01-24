namespace NoSql.Test;

[TestClass]
public class Select_ConstantValue
{
    [TestMethod]
    public async Task Constant()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => double.Pi);
        Assert.AreEqual(double.Pi, value);
    }

    [TestMethod]
    public async Task Static_Field()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => StaticFieldValue);
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Static_Property()
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => StaticPropertyValue);
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    [DataRow(1)]
    public async Task Argument(int v)
    {
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => v);
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Argument_Object()
    {
        var args = new Obj
        {
            A = 1
        };
        var value = await DB.Table<PrimitiveValueQueryTestObject>().FindOneAsync(x => args.A);
        Assert.AreEqual(1, value);
    }

    public readonly static int StaticFieldValue = 1;
    public static int StaticPropertyValue { get; } = 1;

    private class Obj
    {
        public int A;
    }
}
