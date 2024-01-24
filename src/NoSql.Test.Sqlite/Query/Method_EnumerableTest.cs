namespace NoSql.Test;

[TestClass]
public class Method_EnumerableTest
{
    [TestMethod]
    public async Task Enumerable_Length()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Length);
        Assert.AreEqual(10, value);
    }

    [TestMethod]
    public async Task Enumerable_Length_Nested()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.ArrayInt.Length);
        Assert.AreEqual(10, value);
    }

    [TestMethod]
    public async Task Enumerable_ArrayIndexAccess()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt[1]);
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt[2]);
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public async Task Enumerable_ArrayIndexAccess_Nested()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.ArrayInt[1]);
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.ArrayInt[2]);
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public async Task Enumerable_ElementAt()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.ElementAt(1));
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.ElementAt(2));
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public async Task Enumerable_ElementAt_Nested()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.ArrayInt.ElementAt(1));
        Assert.AreEqual(1, value);
        value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NestedObject.ArrayInt.ElementAt(2));
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public async Task Enumerable_Max()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Max());
        Assert.AreEqual(9, value);
    }

    [TestMethod]
    public async Task Enumerable_Max_WithParam()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayObject.Max(x => x.Int));
        Assert.AreEqual(9, value);
    }

    [TestMethod]
    public async Task Enumerable_Min()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Min());
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Enumerable_Min_WithParam()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayObject.Min(x => x.Int));
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task Enumerable_Sum()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Sum());
        Assert.AreEqual(45, value);
    }

    [TestMethod]
    public async Task Enumerable_Sum_WithParam()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayObject.Sum(x => x.Int));
        Assert.AreEqual(45, value);
    }

    [TestMethod]
    public async Task Enumerable_Average()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Average());
        Assert.AreEqual(4.5, value);
    }

    [TestMethod]
    public async Task Enumerable_Average_WithParam()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayObject.Average(x => x.Int));
        Assert.AreEqual(4.5, value);
    }


    [TestMethod]
    public async Task Enumerable_Any_WithParameter_InWhereClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.Any(x => x > 1)).AnyAsync();
        Assert.AreEqual(true, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.Any(x => x > 10)).AnyAsync();
        Assert.AreEqual(false, value);
    }

    [TestMethod]
    public async Task Enumerable_Any_InWhereClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.Any()).AnyAsync();
        Assert.AreEqual(true, value);
    }

    [TestMethod]
    public async Task Enumerable_All_InWhereClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.All(y => y > 1)).AnyAsync();
        Assert.AreEqual(false, value);
        value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.All(y => y >= 0)).AnyAsync();
        Assert.AreEqual(true, value);
    }

    [TestMethod]
    public async Task Enumerable_Any_InSelectClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Any());
        Assert.AreEqual(true, value);
        value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Any(x => x < 0));
        Assert.AreEqual(false, value);
    }

    [TestMethod]
    public async Task Enumerable_Count_WithParameter_InWhereClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.Count(x => x >= 1) == 9).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public async Task Enumerable_Count_InSelectClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Count());
        Assert.AreEqual(10, value);
    }

    [TestMethod]
    public async Task Enumerable_Count_WithParameter_InSelectClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.ArrayInt.Count(y => y >= 1));
        Assert.AreEqual(9, value);
    }

    [TestMethod]
    public async Task Enumerable_Count_InWhereClause()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().Where(x => x.ArrayInt.Count() == 10).CountAsync();
        Assert.AreEqual(1, value);
    }

    [TestMethod]
    public void Enumerable_Select()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Select(y => y.Int),
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_Where()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Where(x => x.Int > 0)
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(9, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_Where_Select()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Where(x => x.Int > 0).Select(y => y.Int)
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(9, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_Where_Where()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Where(x => x.Int > 0).Where(x => x.Int < 9)
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(8, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_Select_Where()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Select(y => y.Int).Where(x => x > 0)
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(9, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_OrderBy()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.OrderBy(x => x.Int).ToArray()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_OrderByDescending()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.OrderByDescending(x => x.Int).ToList()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_Take()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Take(3).ToList()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(3, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_Skip()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.Skip(3).ToList()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(7, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_ToList()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.ToList()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_ToArray()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.ToArray()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Ints.Count());
    }

    [TestMethod]
    public void Enumerable_ToHashSet()
    {
        var value = DB.Table<JsonValueQueryTestObject>().FindOne(x => new
        {
            Ints = x.ArrayObject.ToHashSet()
        });

        Assert.IsNotNull(value);
        Assert.AreEqual(10, value.Ints.Count());
    }
}
