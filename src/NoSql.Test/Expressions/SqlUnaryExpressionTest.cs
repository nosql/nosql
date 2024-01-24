using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlUnaryExpressionTest
{
    [TestMethod]
    public void LogicalNot()
    {
        var expression = new SqlUnaryExpression(typeof(bool), null,
             ExpressionType.Not,
             new SqlColumnExpression(typeof(bool), null, "x"));
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"NOT(\"x\")", sql);
    }

    [TestMethod]
    public void ArithmeticNot()
    {
        var expression = new SqlUnaryExpression(typeof(int), null,
             ExpressionType.Not,
             new SqlColumnExpression(typeof(int), null, "x"));
        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual($"~\"x\"", sql);
    }

    [TestMethod]
    public void Negate()
    {
        var expression = new SqlUnaryExpression(typeof(int), null,
             ExpressionType.Negate,
             new SqlColumnExpression(typeof(int), null, "x"));
        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual($"-\"x\"", sql);
    }

    [TestMethod]
    public void Cast_Implicit()
    {
        Expression<Func<Table, long>> expression = t => t.Id;
        var sql = SqlGeneratorTester.Generate<Table>(expression);
        Assert.AreEqual("CAST(\"Id\" AS Int64)", sql);
    }

    [TestMethod]
    public void Cast_Explicit()
    {
        Expression<Func<Table, object>> expression = t => (long)t.Id;
        var sql = SqlGeneratorTester.Generate<Table>(expression);
        Assert.AreEqual("CAST(CAST(\"Id\" AS Int64) AS Object)", sql);
    }

    [TestMethod]
    [DataRow("IS NULL", ExpressionType.Equal)]
    [DataRow("IS NOT NULL", ExpressionType.NotEqual)]
    public void IsNullOrIsNotNull(string op, ExpressionType opType)
    {
        var expression = new SqlUnaryExpression(typeof(bool), null,
            opType,
            new SqlColumnExpression(typeof(int), null, "x"));
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"\"x\" {op}", sql);
    }

    private class Table
    {
        public int Id { get; set; }
    }
}
