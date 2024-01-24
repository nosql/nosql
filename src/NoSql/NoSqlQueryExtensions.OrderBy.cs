using NoSql.Query.Parser;

namespace NoSql;

public static partial class NoSqlQueryExtensions
{
    public static NoSqlQuery<T> OrderBy<T>(this NoSqlQuery<T> selector, string property, bool descending = false)
    {
        var booltype = selector.Dependencies.TypeMappingSource.FindMapping(typeof(object));
        JsonPathParser parser = new(property, booltype);
        var columnExpression = parser.Parse();
        return selector.OrderBy(columnExpression, descending);
    }

    public static NoSqlQuery<T> OrderByDescending<T, TResult>(this NoSqlQuery<T> selector, string property) => selector.OrderBy(property, true);
}