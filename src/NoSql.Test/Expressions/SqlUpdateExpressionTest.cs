using System.Linq.Expressions;
using NoSql.Query.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlUpdateExpressionTest
{
    [TestMethod]
    public void SetConstantValue()
    {
        var expression = new SqlUpdateExpression(
            new List<SqlColumnValueSetExpression>
            {
                new SqlColumnValueSetExpression(new SqlColumnExpression(typeof(int), null, "C1"), SqlGeneratorTester.Constant(1) ),
                new SqlColumnValueSetExpression(new SqlColumnExpression(typeof(string), null, "C2"), SqlGeneratorTester.Constant("a") )
            },
            new SqlTableExpression(typeof(Table), "T1"),
            null);

        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual("UPDATE \"T1\" SET \"C1\" = 1, \"C2\" = 'a'", sql);
    }

    [TestMethod]
    public void SetColumnValue()
    {
        var expression = new SqlUpdateExpression(
            new List<SqlColumnValueSetExpression>
            {
                new SqlColumnValueSetExpression(new SqlColumnExpression(typeof(int), null, "C1"), new SqlColumnExpression(typeof(int), null, "C2")),
                new SqlColumnValueSetExpression(new SqlColumnExpression(typeof(string), null, "C3"), SqlGeneratorTester.Constant("a") )
            },
            new SqlTableExpression(typeof(Table), "T1"),
            null);

        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual("UPDATE \"T1\" SET \"C1\" = \"C2\", \"C3\" = 'a'", sql);
    }

    [TestMethod]
    public void SetComputeValue()
    {
        var expression = new SqlUpdateExpression(
            new List<SqlColumnValueSetExpression>
            {
                new SqlColumnValueSetExpression(
                    new SqlColumnExpression(typeof(int), null,"C1"),
                    new SqlBinaryExpression(typeof(int), null,
                        ExpressionType.Add,
                        SqlGeneratorTester.Constant(1) ,
                        new SqlColumnExpression(typeof(int), null,"C2")))
            },
            new SqlTableExpression(typeof(Table), "T1"),
            null);

        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual("UPDATE \"T1\" SET \"C1\" = 1 + \"C2\"", sql);
    }

    private class Table { }
}
