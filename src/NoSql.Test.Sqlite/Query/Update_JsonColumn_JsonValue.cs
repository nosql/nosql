namespace NoSql.Test;

[TestClass]
public class Update_JsonColumn_JsonValue
{
    [TestMethod]
    public async Task Set_Array()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.ArrayInt, new int[] { 1, 2, 3 }));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync(x => x.ArrayInt);
        Assert.IsNotNull(v);
        Assert.AreEqual(3, v.Length);
        Assert.AreEqual(1, v[0]);
        Assert.AreEqual(2, v[1]);
        Assert.AreEqual(3, v[2]);
    }

    [TestMethod]
    public async Task Merge_Object()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(x => new JsonValueUpdateTestObject
        {
            Object = new PrimitiveValueQueryTestObject
            {
                Int = 3,
                Long = 4,
            }
        });
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync();
        Assert.IsNotNull(v);
        Assert.IsNotNull(v.Object);
        Assert.AreEqual(3, v.Object.Int);
        Assert.AreEqual(4, v.Object.Long);


        Assert.AreEqual(sbyte.MaxValue, v.Object.SByte);
        Assert.AreEqual(byte.MaxValue, v.Object.Byte);
        Assert.AreEqual(short.MaxValue, v.Object.Short);
    }

    [TestMethod]
    public async Task Merge_Increment()
    {
        await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Increment, 1));

        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(x => new JsonValueUpdateTestObject
        {
            Object = new PrimitiveValueQueryTestObject
            {
                Increment = x.Object.Increment + 1,
            }
        });
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync();
        Assert.IsNotNull(v);
        Assert.IsNotNull(v.Object);
        Assert.AreEqual(2, v.Object.Increment);


        Assert.AreEqual(sbyte.MaxValue, v.Object.SByte);
        Assert.AreEqual(byte.MaxValue, v.Object.Byte);
        Assert.AreEqual(short.MaxValue, v.Object.Short);
    }

    [TestMethod]
    public async Task Set_Object_Increment()
    {
        await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object.Increment, 1));

        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Object, x => new()
        {
            Increment = x.Object.Increment + 1
        }));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync();
        Assert.IsNotNull(v);
        Assert.IsNotNull(v.Object);
        Assert.AreEqual(2, v.Object.Increment);

        Assert.AreEqual(0, v.Object.SByte);
        Assert.AreEqual(0, v.Object.Byte);
        Assert.AreEqual(0, v.Object.Short);
    }


    [TestMethod]
    public async Task Set_NestedObject_New()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.NestedObject.Object, x => new PrimitiveValueUpdateTestObject
        {
            Int = 3
        }));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync();
        Assert.IsNotNull(v);
        Assert.AreEqual(3, v.NestedObject.Object.Int);

        Assert.AreEqual(0, v.NestedObject.Object.SByte);
        Assert.AreEqual(0, v.NestedObject.Object.Byte);
        Assert.AreEqual(0, v.NestedObject.Object.Short);
    }

    [TestMethod]
    public async Task Set_Object_New()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.UpdateObject, x => new()
        {
            Int = 3
        }));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync();
        Assert.IsNotNull(v);
        Assert.AreEqual(false, v.UpdateObject.Bool);
        Assert.AreEqual(3, v.UpdateObject.Int);
        Assert.AreEqual(null, v.UpdateObject.String);
    }

    [TestMethod]
    public async Task Set_Object_MemberInit()
    {
        var r = await DB.Table<JsonValueUpdateTestObject>().ExecuteUpdateAsync(setter => setter.SetProperty(x => x.UpdateObject, x => new()
        {
            Int = 3
        }));
        Assert.AreEqual(1, r);

        var v = await DB.Table<JsonValueUpdateTestObject>().FindOneAsync();
        Assert.IsNotNull(v);
        Assert.AreEqual(false, v.UpdateObject.Bool);
        Assert.AreEqual(3, v.UpdateObject.Int);
        Assert.AreEqual(null, v.UpdateObject.String);
    }
}
