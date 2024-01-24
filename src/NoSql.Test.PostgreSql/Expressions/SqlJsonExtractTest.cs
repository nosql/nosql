using NoSql.Query.Expressions;

namespace NoSql.Test.PostgreSql;

[TestClass]
public class SqlJsonExtractTest
{
    [TestMethod]
    [DataRow("jsonb_extract_path_text(\"Data\")", "Data")]
    [DataRow("jsonb_extract_path_text(\"Data\",'a')", "Data", "a")]
    [DataRow("jsonb_extract_path_text(\"Data\",'a','b')", "Data", "a", "b")]
    public void Extract_StringValue(string expectSql, string column, params string[] members)
    {
        var exp = new SqlJsonExtractExpression(typeof(string), null,
            new SqlColumnExpression(typeof(object), null, column),
            members);
        var sql = DB.Generate(exp);
        Assert.AreEqual(expectSql, sql);
    }

    [TestMethod]
    [DataRow("jsonb_extract_path_text(\"Data\")", "Data")]
    [DataRow("jsonb_extract_path_text(\"Data\",'a')", "Data", "a")]
    [DataRow("jsonb_extract_path_text(\"Data\",'a','b')", "Data", "a", "b")]
    public void Extract_IntegerValue(string expectSql, string column, params string[] members)
    {
        var exp = new SqlJsonExtractExpression(typeof(int), null,
            new SqlColumnExpression(typeof(object), null, column),
            members);
        var sql = DB.Generate(exp);
        Assert.AreEqual(expectSql, sql);
    }

    [TestMethod]
    [DataRow("jsonb_extract_path_text(\"Data\")", "Data")]
    [DataRow("jsonb_extract_path_text(\"Data\",'a')", "Data", "a")]
    [DataRow("jsonb_extract_path_text(\"Data\",'a','b')", "Data", "a", "b")]
    public void Extract_Object(string expectSql, string column, params string[] members)
    {
        var exp = new SqlJsonExtractExpression(typeof(object), null,
            new SqlColumnExpression(typeof(object), null, column),
            members);
        var sql = DB.Generate(exp);
        Assert.AreEqual(expectSql, sql);
    }

}
