
/* 项目“NoSql (net6.0)”的未合并的更改
在此之前:
namespace NoSql.Query.Expressions;
在此之后:
using NoSql;
using NoSql.Query;
using NoSql.Query;
using NoSql.Query.Expressions;
*/
using NoSql.Query.Expressions;

namespace NoSql.Query;

public readonly struct PathSegment
{
    public PathSegment(string propertyName)
    {
        PropertyName = propertyName;
        ArrayIndex = null;
    }

    public PathSegment(SqlExpression arrayIndex)
    {
        ArrayIndex = arrayIndex;
        PropertyName = null;
    }

    public string? PropertyName { get; }

    public SqlExpression? ArrayIndex { get; }

    public static implicit operator PathSegment(string propertyName) => new(propertyName);
}
