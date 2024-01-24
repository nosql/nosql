using NoSql.Query.Expressions;

namespace NoSql.Query;

public interface ISqlTranslatingExpressionVisitorFactory
{
    ISqlTranslatingExpressionVisitor Create(IDictionary<string, SqlExpression>? parameters = null);

    ISqlTranslatingExpressionVisitor Create(string name, SqlExpression expression) => Create(new Dictionary<string, SqlExpression>
    {
        { name, expression }
    });

}
