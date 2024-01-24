using NoSql.Query.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlColumnExpressionTest
{
    [TestMethod]
    public void Column()
    {
        var column = new SqlColumnExpression(typeof(int), null, "Id");
        var sql = SqlGeneratorTester.Generate(column);
        Assert.AreEqual("\"Id\"", sql);
    }
}
