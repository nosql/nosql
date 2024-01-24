using System.Linq.Expressions;

namespace NoSql.Test.Sqlite;

[TestClass]
public class ConstantExpressionTest
{
    [TestMethod]
    [DataRow(null, "NULL")]
    [DataRow(true, "1")]
    [DataRow(false, "0")]
    [DataRow(1, "1")]
    [DataRow(1.1, "1.1000000000000001")]
    [DataRow(1.1f, "1.1")]
    [DataRow((byte)1, "1")]
    [DataRow((short)1, "1")]
    [DataRow((long)1, "1")]
    [DataRow((sbyte)1, "1")]
    [DataRow((uint)1, "1")]
    [DataRow((ulong)1, "1")]
    [DataRow("abc", "'abc'")]
    [DataRow('c', "'c'")]
    public void Constant(object? value, string sql)
    {
        Assert.AreEqual(sql, DB.Generate(Expression.Constant(value)));
    }

}
