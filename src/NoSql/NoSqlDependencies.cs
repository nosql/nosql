using NoSql.Query;
using NoSql.Scaffolding;
using NoSql.Storage;

namespace NoSql;

public class NoSqlDependencies
{
    public NoSqlDependencies(
                              INoSqlDbConnection connection,
                              ISqlGeneratorFactory generatorFactory,
                              ISqlTypeMappingSource typeMappingSource,
                              IDatabaseFactory tableCreator,
                              ITypeInfoResolver tableInfoResolver,
                              ISqlTranslatingExpressionVisitorFactory translatingFactory,
                              ISqlExpressionFactory expressionFactory)
    {
        Connection = connection;
        GeneratorFactory = generatorFactory;
        TypeMappingSource = typeMappingSource;
        TableCreator = tableCreator;
        TableInfoResolver = tableInfoResolver;
        TranslatingFactory = translatingFactory;
        ExpressionFactory = expressionFactory;
    }

    public INoSqlDbConnection Connection { get; }
    public ISqlGeneratorFactory GeneratorFactory { get; }
    public ISqlTypeMappingSource TypeMappingSource { get; }
    public IDatabaseFactory TableCreator { get; }
    public ITypeInfoResolver TableInfoResolver { get; }
    public ISqlTranslatingExpressionVisitorFactory TranslatingFactory { get; }
    public ISqlExpressionFactory ExpressionFactory { get; }
}

public sealed class SqlDatabaseDependencies<TDatabase> : NoSqlDependencies
{
    public SqlDatabaseDependencies(INoSqlDbConnection connection,
                               ISqlGeneratorFactory generatorFactory,
                               ISqlTypeMappingSource typeMappingSource,
                               IDatabaseFactory tableCreator,
                               ITypeInfoResolver tableInfoResolver,
                               ISqlTranslatingExpressionVisitorFactory translatingFactory,
                               ISqlExpressionFactory expressionFactory)
        : base(connection, generatorFactory, typeMappingSource, tableCreator, tableInfoResolver, translatingFactory, expressionFactory)
    {
    }
}
