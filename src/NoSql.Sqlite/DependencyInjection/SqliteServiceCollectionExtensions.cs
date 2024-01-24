using Microsoft.Extensions.DependencyInjection;
using NoSql.Query;
using NoSql.Query.Translators;
using NoSql.Scaffolding;
using NoSql.Sqlite.Query;
using NoSql.Sqlite.Query.Translators;
using NoSql.Sqlite.Scaffolding;
using NoSql.Sqlite.Storage;
using NoSql.Storage;

namespace NoSql;

public static class SqliteServiceCollectionExtensions
{
    public static NoSqlBuilder UseSqlite(this NoSqlBuilder builder, string connectionString)
    {
        builder.Services.Add(new ServiceDescriptor(typeof(INoSqlDbConnection), p => new SqliteDbConnection(connectionString), builder.Lifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(IDatabaseFactory), typeof(SqliteDatabaseFactory), builder.Lifetime));

        builder.Services.AddSingleton<ISqlGeneratorFactory, SqliteSqlGeneratorFactory>();
        builder.Services.AddSingleton<ISqlExpressionFactory, SqliteSqlExpressionFactory>();
        builder.Services.AddSingleton<ISqlTypeMappingSource, SqliteTypeMappingSource>();

        builder.Services.AddSingleton<IMemberTranslator, SqliteStringMemberTranslator>();
        builder.Services.AddSingleton<IMemberTranslator, SqliteDateTimeFunctionTranslator>();

        return builder;
    }
}