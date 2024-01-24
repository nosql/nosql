using NoSql.Query.Expressions;
using NoSql.Query.Parser;
using System.Linq.Expressions;

namespace NoSql;

public static partial class NoSqlQueryExtensions
{
    public static NoSqlQuery<T> WhereEq<T>(this NoSqlQuery<T> selector, string property, object value) => selector.Where(property, ExpressionType.Equal, value);
    public static NoSqlQuery<T> WhereNeq<T>(this NoSqlQuery<T> selector, string property, object value) => selector.Where(property, ExpressionType.NotEqual, value);
    public static NoSqlQuery<T> WhereGt<T>(this NoSqlQuery<T> selector, string property, object value) => selector.Where(property, ExpressionType.GreaterThan, value);
    public static NoSqlQuery<T> WhereGte<T>(this NoSqlQuery<T> selector, string property, object value) => selector.Where(property, ExpressionType.GreaterThanOrEqual, value);
    public static NoSqlQuery<T> WhereLt<T>(this NoSqlQuery<T> selector, string property, object value) => selector.Where(property, ExpressionType.LessThan, value);
    public static NoSqlQuery<T> WhereLte<T>(this NoSqlQuery<T> selector, string property, object value) => selector.Where(property, ExpressionType.LessThanOrEqual, value);

    public static NoSqlQuery<T> WhereNot<T>(this NoSqlQuery<T> selector, string property)
    {
        var type = selector.Dependencies.TypeMappingSource.FindMapping(typeof(bool));
        JsonPathParser parser = new(property, type);
        var columnExpression = parser.Parse();
        return selector.Where(new SqlUnaryExpression(type, ExpressionType.Not, columnExpression));
    }

    public static NoSqlQuery<T> WhereLike<T>(this NoSqlQuery<T> selector, string property, string value)
    {
        var type = selector.Dependencies.TypeMappingSource.FindMapping(typeof(bool));
        JsonPathParser parser = new(property, type);
        var columnExpression = parser.Parse();
        return selector.Where(new SqlLikeExpression(type, columnExpression, value));
    }

    private static NoSqlQuery<T> Where<T>(this NoSqlQuery<T> selector, string property, ExpressionType expressionType, object value)
    {
        var type = selector.Dependencies.TypeMappingSource.FindMapping(value.GetType());
        JsonPathParser parser = new(property, type);
        var columnExpression = parser.Parse();
        return selector.Where(new SqlBinaryExpression(typeof(bool), null, expressionType, columnExpression, new SqlConstantExpression(type, value)));
    }

    public static NoSqlQuery<T> WhereAnyLike<T>(this NoSqlQuery<T> selector, string property, string value)
    {
        var booltype = selector.Dependencies.TypeMappingSource.FindMapping(typeof(bool));
        JsonPathParser parser = new(property, booltype);
        var columnExpression = parser.Parse();

        return selector.Where(new SqlExistsExpression(booltype,
            new SqlSelectExpression(
                new SqlProjectionExpression(new SqlFragmentExpression("1")),
                new SqlJsonArrayEachExpression(columnExpression),
                new SqlLikeExpression(booltype, columnExpression, $"%{value}%")
                )));
    }
}

