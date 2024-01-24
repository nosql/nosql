using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql.Query;

public interface ISqlTranslatingExpressionVisitor
{
    SqlExpression Visit(Expression expression);
}
