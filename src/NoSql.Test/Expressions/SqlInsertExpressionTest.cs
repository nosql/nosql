using System.Linq.Expressions;
using NoSql.Query.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlInsertExpressionTest
{
    [TestMethod]
    public void Single_Column()
    {
        var expression = new SqlInsertExpression(
            new SqlTableExpression(typeof(Table), "T1"),
            new Dictionary<SqlColumnExpression, SqlExpression>
            {
                {
                    new SqlColumnExpression(typeof(int), null, "C1"),
                    SqlGeneratorTester.Constant(0)
                },
            });

        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual("INSERT INTO \"T1\" (\"C1\") VALUES(0)", sql);
    }

    [TestMethod]
    public void Muliple_Column()
    {
        var expression = new SqlInsertExpression(
            new SqlTableExpression(typeof(Table), "T1"),
            new Dictionary<SqlColumnExpression, SqlExpression>
            {
                { new SqlColumnExpression(typeof(int), null, "C1"), SqlGeneratorTester.Constant(0) },
                { new SqlColumnExpression(typeof(string), null, "C2"), SqlGeneratorTester.Constant("a") },
            });

        var sql = SqlGeneratorTester.Generate(expression);

        Assert.AreEqual("INSERT INTO \"T1\" (\"C1\",\"C2\") VALUES(0,'a')", sql);
    }

    private class Table { }
}
