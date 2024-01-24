namespace NoSql.Test;

[TestClass]
public class Update_MultipleColumn
{
    [TestMethod]
    public async Task Set_ValueColumn()
    {
        var r = await DB.Table<PrimitiveValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter
            .SetProperty(x => x.Bool, x => false)
            .SetProperty(x => x.Int, x => -1)
            .SetProperty(x => x.String, (string?)null));

        Assert.AreEqual(1, r);

        var v = await DB.Table<PrimitiveValueUpdateTestObject>().FindOneAsync(x => new { x.Bool, x.Int, x.String });
        Assert.IsNotNull(v);
        Assert.AreEqual(false, v.Bool);
        Assert.AreEqual(-1, v.Int);
        Assert.AreEqual(null, v.String);
    }

    [TestMethod]
    public async Task Set_ValueColumn_And_JsonColumn()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter
            .SetProperty(x => x.ArrayInt, new int[] { 1, 2, 3 })
            .SetProperty(x => x.Object.Bool, x => false)
            .SetProperty(x => x.Object.Int, x => -1)
            .SetProperty(x => x.Object.String, (string?)null));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => new { x.Object.Bool, x.Object.Int, x.Object.String, x.ArrayInt });
        Assert.IsNotNull(v);
        Assert.AreEqual(false, v.Bool);
        Assert.AreEqual(-1, v.Int);
        Assert.AreEqual(null, v.String);
        Assert.IsNotNull(v.ArrayInt);
        Assert.AreEqual(3, v.ArrayInt.Length);
    }

    [TestMethod]
    public async Task Merge_JsonColumn()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(x => new JsonValueUpdateTestObject
        {
            ArrayInt = new int[] { 1, 2, 3 },
            Object = new PrimitiveValueQueryTestObject
            {
                Bool = false,
                Int = -1,
                String = null,
            }
        });

        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => new { x.Object.Bool, x.Object.Int, x.Object.String, x.ArrayInt });
        Assert.IsNotNull(v);
        Assert.AreEqual(false, v.Bool);
        Assert.AreEqual(-1, v.Int);
        Assert.AreEqual(null, v.String);
        Assert.IsNotNull(v.ArrayInt);
        Assert.AreEqual(3, v.ArrayInt.Length);
    }

}
