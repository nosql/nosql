using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlBinaryExpressionTest
{
    [TestMethod]
    [DataRow("=", ExpressionType.Equal)]
    [DataRow("<>", ExpressionType.NotEqual)]
    [DataRow(">", ExpressionType.GreaterThan)]
    [DataRow(">=", ExpressionType.GreaterThanOrEqual)]
    [DataRow("<", ExpressionType.LessThan)]
    [DataRow("<=", ExpressionType.LessThanOrEqual)]
    public void Compare(string op, ExpressionType opType)
    {
        var expression = new SqlBinaryExpression(typeof(bool), null,
            opType,
            new SqlColumnExpression(typeof(int), null, "x"),
            SqlGeneratorTester.Constant(1));
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"\"x\" {op} 1", sql);
    }

    [TestMethod]
    [DataRow("AND", ExpressionType.AndAlso)]
    [DataRow("OR", ExpressionType.OrElse)]
    public void Logical(string op, ExpressionType opType)
    {
        var expression = new SqlBinaryExpression(typeof(bool), null,
            opType,
            new SqlColumnExpression(typeof(bool), null, "x"),
            SqlGeneratorTester.Constant(true));
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"\"x\" {op} 1", sql);
    }

}