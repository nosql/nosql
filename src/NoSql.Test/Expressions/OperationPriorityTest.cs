using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class OperationPriorityTest
{
    [TestMethod]
    public void And_Or()
    {
        var expression = new SqlBinaryExpression(typeof(bool), null,
            ExpressionType.AndAlso,
            new SqlBinaryExpression(typeof(bool), null,
                ExpressionType.OrElse,
                new SqlColumnExpression(typeof(bool), null, "x"),
                SqlGeneratorTester.Constant(true)),
            new SqlBinaryExpression(typeof(bool), null,
                ExpressionType.OrElse,
                new SqlColumnExpression(typeof(bool), null, "y"),
                SqlGeneratorTester.Constant(false)));
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"(\"x\" OR 1) AND (\"y\" OR 0)", sql);
    }

    [TestMethod]
    public void Or_And()
    {
        var expression = new SqlBinaryExpression(typeof(bool), null,
            ExpressionType.OrElse,
            new SqlBinaryExpression(typeof(bool), null,
                ExpressionType.AndAlso,
                new SqlColumnExpression(typeof(bool), null, "x"),
                SqlGeneratorTester.Constant(true)),
            new SqlBinaryExpression(typeof(bool), null,
                ExpressionType.AndAlso,
                new SqlColumnExpression(typeof(bool), null, "y"),
                SqlGeneratorTester.Constant(false)));

        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"(\"x\" AND 1) OR (\"y\" AND 0)", sql);
    }

    [TestMethod]
    public void Multiply__Add_Subtract()
    {
        var expression = new SqlBinaryExpression(typeof(int), null,
            ExpressionType.Multiply,
            new SqlBinaryExpression(typeof(int), null,
                ExpressionType.Add,
                new SqlColumnExpression(typeof(int), null, "x"),
                SqlGeneratorTester.Constant(1)),
            new SqlBinaryExpression(typeof(int), null,
                ExpressionType.Subtract,
                new SqlColumnExpression(typeof(int), null, "y"),
                SqlGeneratorTester.Constant(2)));

        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual($"(\"x\" + 1) * (\"y\" - 2)", sql);
    }

    //[TestMethod]
    //public void Add__Multiply_Divide()
    //{
    //    var expression = Expression.Add(
    //        Expression.Multiply(
    //            new SqlColumnExpression(typeof(int), "x"),
    //            Expression.Constant(1)),
    //        Expression.Divide(
    //            new SqlColumnExpression(typeof(int), "y"),
    //            Expression.Constant(2)));
    //    var sql = SqlGeneratorTester.Generate(expression);
    //    Assert.AreEqual($"\"x\" * 1 + \"y\" / 2", sql);
    //}

}
