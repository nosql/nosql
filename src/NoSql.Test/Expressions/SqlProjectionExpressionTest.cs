using NoSql.Query.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlProjectionExpressionTest
{
    [TestMethod]
    public void Projection()
    {
        var expression = new SqlProjectionExpression(new SqlColumnExpression(typeof(int), null, "C1"));
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("\"C1\"", sql);
    }

    [TestMethod]
    public void Projection_Alias()
    {
        var expression = new SqlProjectionExpression(new SqlColumnExpression(typeof(int), null, "C1"), "A1");
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("\"C1\" AS 'A1'", sql);
    }

    [TestMethod]
    public void Projection_SameAlias()
    {
        var expression = new SqlProjectionExpression(new SqlColumnExpression(typeof(int), null, "C1"), "C1");
        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("\"C1\" AS 'C1'", sql);
    }

    //[TestMethod]
    //public void Projection_List()
    //{
    //    var expression = new SqlProjectionListExpression(typeof(Table), new SqlProjectionExpression[]
    //    {
    //        new SqlProjectionExpression(new SqlColumnExpression(typeof(int), nameof(Table.C1), null), "C1"),
    //        new SqlProjectionExpression(new SqlColumnExpression(typeof(bool), nameof(Table.C2), null)),
    //        new SqlProjectionExpression(new SqlColumnExpression(typeof(bool), nameof(Table.C3), null), "A3"),
    //    }, null);
    //    var sql = SqlGeneratorTester.Generate(expression);
    //    Assert.AreEqual("\"C1\" AS 'C1',\"C2\",\"C3\" AS 'A3'", sql);
    //}

    public class Table
    {
        public int C1 { get; set; }
        public bool C2 { get; set; }
        public string? C3 { get; set; }
    }

}
