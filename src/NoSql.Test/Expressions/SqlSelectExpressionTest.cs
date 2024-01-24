using NoSql.Query.Expressions;

namespace NoSql.Expressions;

[TestClass]
public class SqlSelectExpressionTest
{
    [TestMethod]
    public void Single_Column()
    {
        var expression = new SqlSelectExpression(
            new SqlProjectionExpression(new SqlColumnExpression(typeof(int), null, nameof(Table.C1)), "C1"),
            new SqlTableExpression(typeof(Table), "T1"));

        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("SELECT \"C1\" AS 'C1' FROM \"T1\"", sql);
    }

    [TestMethod]
    public void Multiple_Column()
    {
        var expression = new SqlSelectExpression(
            new SqlProjectionListExpression(
                SqlGeneratorTester.TypeInfoResolver.GetTypeInfo(typeof(Table)).TypeMapping,
                new SqlProjectionExpression[]
                {
                    new SqlProjectionExpression(new SqlColumnExpression(typeof(int), null, nameof(Table.C1)),"C1"),
                    new SqlProjectionExpression(new SqlColumnExpression(typeof(bool), null, nameof(Table.C2))),
                }),
            new SqlTableExpression(typeof(Table), "T1"));

        var sql = SqlGeneratorTester.Generate(expression);
        Assert.AreEqual("SELECT \"C1\" AS 'C1',\"C2\" FROM \"T1\"", sql);
    }

    public class Table
    {
        public int C1 { get; set; }
        public bool C2 { get; set; }
        public string? C3 { get; set; }
    }
}
