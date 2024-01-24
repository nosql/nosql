using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.Storage;

namespace NoSql.Query;

public class SqlTranslatingExpressionVisitorFactory : ISqlTranslatingExpressionVisitorFactory
{
    private readonly NoSqlOptions _options;
    private readonly ISqlTypeMappingSource _typeMappingSource;
    private readonly ISqlExpressionTranslatorProvider _translatorProvider;

    public SqlTranslatingExpressionVisitorFactory(NoSqlOptions options,
                                                  ISqlTypeMappingSource typeMappingSource,
                                                  ISqlExpressionTranslatorProvider translatorProvider)
    {
        _options = options;
        _typeMappingSource = typeMappingSource;
        _translatorProvider = translatorProvider;
    }

    public ISqlTranslatingExpressionVisitor Create(IDictionary<string, SqlExpression>? parameters = null)
    {
        return new SqlTranslatingExpressionVisitor(_options, this, _typeMappingSource, _translatorProvider, parameters);
    }
}
