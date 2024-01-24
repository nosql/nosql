using NoSql.Query.Expressions;
using NoSql.Query.Parser;
using NoSql.Storage;
using System.Data.Common;

namespace NoSql.Parser;

[TestClass]
public class SqlExpressionParserTest
{
    [TestMethod]
    public void Column()
    {
        JsonPathParser parser = new("a", new ValueTypeMapping(typeof(bool), "bool"));
        var exp = parser.Parse();
        Assert.IsInstanceOfType(exp, typeof(SqlColumnExpression));
        Assert.AreEqual("a", ((SqlColumnExpression)exp).Name);
    }

    [TestMethod]
    public void Path_Property()
    {
        JsonPathParser parser = new("a.b.c", new ValueTypeMapping(typeof(bool), "bool"));
        var exp = parser.Parse();
        Assert.IsInstanceOfType(exp, typeof(SqlJsonExtractExpression));
        Assert.IsInstanceOfType(((SqlJsonExtractExpression)exp).Column, typeof(SqlColumnExpression));
        Assert.AreEqual("a", ((SqlColumnExpression)((SqlJsonExtractExpression)exp).Column).Name);
        Assert.AreEqual(2, ((SqlJsonExtractExpression)exp).Path.Length);
        Assert.AreEqual("b", ((SqlJsonExtractExpression)exp).Path[0].PropertyName);
        Assert.AreEqual("c", ((SqlJsonExtractExpression)exp).Path[1].PropertyName);
    }

    [TestMethod]
    public void Path_Index()
    {
        JsonPathParser parser = new("a.b[1]", new ValueTypeMapping(typeof(bool), "bool"));
        var exp = parser.Parse();
        Assert.IsInstanceOfType(exp, typeof(SqlJsonExtractExpression));
        Assert.IsInstanceOfType(((SqlJsonExtractExpression)exp).Column, typeof(SqlColumnExpression));
        Assert.AreEqual("a", ((SqlColumnExpression)((SqlJsonExtractExpression)exp).Column).Name);
        Assert.AreEqual(2, ((SqlJsonExtractExpression)exp).Path.Length);
        Assert.AreEqual("b", ((SqlJsonExtractExpression)exp).Path[0].PropertyName);

        var index = ((SqlJsonExtractExpression)exp).Path[1].ArrayIndex;
        Assert.IsInstanceOfType(index, typeof(SqlConstantExpression));
        Assert.AreEqual(1, ((SqlConstantExpression)index).Value);
    }

    [TestMethod]
    public void Path_Index_Property()
    {
        JsonPathParser parser = new("a.b[1].c", new ValueTypeMapping(typeof(bool), "bool"));
        var exp = parser.Parse();
        Assert.IsInstanceOfType(exp, typeof(SqlJsonExtractExpression));
        Assert.IsInstanceOfType(((SqlJsonExtractExpression)exp).Column, typeof(SqlColumnExpression));
        Assert.AreEqual("a", ((SqlColumnExpression)((SqlJsonExtractExpression)exp).Column).Name);
        Assert.AreEqual(3, ((SqlJsonExtractExpression)exp).Path.Length);
        Assert.AreEqual("b", ((SqlJsonExtractExpression)exp).Path[0].PropertyName);
        Assert.AreEqual("c", ((SqlJsonExtractExpression)exp).Path[2].PropertyName);

        var index = ((SqlJsonExtractExpression)exp).Path[1].ArrayIndex;
        Assert.IsInstanceOfType(index, typeof(SqlConstantExpression));
        Assert.AreEqual(1, ((SqlConstantExpression)index).Value);
    }

    public class ValueTypeMapping : TypeMapping
    {
        public ValueTypeMapping(Type clrType, string storeType) : base(clrType, storeType)
        {
        }

        public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
        {
            throw new NotImplementedException();
        }
    }
}
