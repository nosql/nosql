using NoSql.Query.Expressions;

namespace NoSql.Test.SqlServer;

[TestClass]
public class SqlJsonExtractTest
{
    [TestMethod]
    [DataRow("JSON_VALUE(\"Data\",'$')", "Data")]
    [DataRow("JSON_VALUE(\"Data\",'$.a')", "Data", "a")]
    [DataRow("JSON_VALUE(\"Data\",'$.a.b')", "Data", "a", "b")]
    public void Extract_Value(string expectSql, string column, params string[] members)
    {
        var exp = new SqlJsonExtractExpression(
            typeof(string), null,
            new SqlColumnExpression(typeof(object), null, column),
            members);
        var sql = DB.Generate(exp);
        Assert.AreEqual(expectSql, sql);
    }

    [TestMethod]
    [DataRow("JSON_QUERY(\"Data\",'$')", "Data")]
    [DataRow("JSON_QUERY(\"Data\",'$.a')", "Data", "a")]
    [DataRow("JSON_QUERY(\"Data\",'$.a.b')", "Data", "a", "b")]
    public void Extract_Object(string expectSql, string column, params string[] members)
    {
        var exp = new SqlJsonExtractExpression(
            typeof(object), null,
            new SqlColumnExpression(typeof(object), null, column),
            members);
        var sql = DB.Generate(exp);
        Assert.AreEqual(expectSql, sql);
    }
}
