using NuGet.Frameworks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoSql.Test;

[TestClass]
public class Insert_Delete
{
    [TestMethod]
    public async Task InsertDelete()
    {
        var i = await DB.Table<InsertDeleteTestObject>().InsertAsync(new InsertDeleteTestObject());
        var d = await DB.Table<InsertDeleteTestObject>().ExecuteDeleteAsync();
        Assert.AreEqual(1, i);
        Assert.AreEqual(1, d);
    }

    [TestMethod]
    public async Task InsertDelete_Batch()
    {
        var i = await DB.Table<InsertDeleteTestObject>().InsertAsync(new[] { new InsertDeleteTestObject(1), new InsertDeleteTestObject(2) });
        var d = await DB.Table<InsertDeleteTestObject>().ExecuteDeleteAsync();
        Assert.AreEqual(2, i);
        Assert.AreEqual(2, d);
    }
}
