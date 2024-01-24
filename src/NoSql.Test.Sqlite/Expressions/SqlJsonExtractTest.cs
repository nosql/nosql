using NoSql.Query.Expressions;

namespace NoSql.Test.Sqlite;

[TestClass]
public class SqlJsonExtractTest
{
    [TestMethod]
    [DataRow("json_extract(\"Data\",'$')", "Data")]
    [DataRow("json_extract(\"Data\",'$.a')", "Data", "a")]
    [DataRow("json_extract(\"Data\",'$.a.b')", "Data", "a", "b")]
    public void Extract(string expectSql, string column, params string[] members)
    {
        var exp = new SqlJsonExtractExpression(typeof(int), null,
            new SqlColumnExpression(typeof(object), null, column),
            members);

        var sql = DB.Generate(exp);
        Assert.AreEqual(expectSql, sql);
    }
}
