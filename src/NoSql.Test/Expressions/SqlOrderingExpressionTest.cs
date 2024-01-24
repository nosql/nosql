using NoSql.Query.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlOrderingExpressionTest
{
    [TestMethod]
    public void OrderBy_Asc()
    {
        var expression = new SqlOrderingExpression(new SqlColumnExpression(typeof(int), null, "C1"), false);
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("\"C1\"", sql);
    }

    [TestMethod]
    public void OrderBy_Desc()
    {
        var expression = new SqlOrderingExpression(new SqlColumnExpression(typeof(int), null, "C1"), true);
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("\"C1\" DESC", sql);
    }
}
