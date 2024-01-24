namespace NoSql.Test;

[TestClass]
public class Select_JsonExtract_NullPropagation
{
    [TestMethod]
    public async Task Bool_Default()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NullObject!.Bool);
        Assert.AreEqual(false, value);
    }

    [TestMethod]
    public async Task Int_Default()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NullObject!.Int);
        Assert.AreEqual(0, value);
    }

    [TestMethod]
    public async Task String_Null()
    {
        var value = await DB.Table<JsonValueQueryTestObject>().FindOneAsync(x => x.NullObject!.String);
        Assert.AreEqual(null, value);
    }

}
