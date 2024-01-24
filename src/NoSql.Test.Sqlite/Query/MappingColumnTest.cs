namespace NoSql.Test;

[TestClass]
public class MappingColumnTest
{
    [TestMethod]
    public async Task Mapped_Where()
    {
        var any = await DB.Table<MappedModel>().Where(x => x.Id == 1).AnyAsync();
        Assert.IsTrue(any);
    }

    [TestMethod]
    public async Task Mapped_Select()
    {
        var id = await DB.Table<MappedModel>().FindOneAsync(x => x.Id);
        Assert.AreEqual(1, id);
    }

    [TestMethod]
    public async Task Mapped_OrderBy()
    {
        var data = await DB.Table<MappedModel>().OrderByDescending(x => x.Id).FindOneAsync();
        Assert.AreEqual(2, data.Id);
    }
}