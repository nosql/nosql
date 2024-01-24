using Microsoft.Extensions.DependencyInjection;
using NoSql.PostgreSql.Query;
using NoSql.PostgreSql.Query.Translators;
using NoSql.PostgreSql.Scaffolding;
using NoSql.PostgreSql.Storage;
using NoSql.Query.Translators;
using NoSql.Scaffolding;
using NoSql.Storage;

namespace NoSql;

public static class PostgreSqlServiceCollectionExtensions
{
    public static NoSqlBuilder UsePostgreSql(this NoSqlBuilder builder, string connectionString)
    {
        builder.Services.Add(new ServiceDescriptor(typeof(INoSqlDbConnection), p => new PostgreSqlDbConnection(connectionString), builder.Lifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(IDatabaseFactory), typeof(PostgreSqlDatabaseFactory), builder.Lifetime));

        builder.Services.AddSingleton<ISqlGeneratorFactory, PostgreSqlGeneratorFactory>();
        builder.Services.AddSingleton<ISqlTypeMappingSource, PostgreSqlTypeMappingSource>();

        builder.Services.AddSingleton<IMemberTranslator, PostgreSqlDateTimeFunctionTranslator>();
        builder.Services.AddSingleton<IMemberTranslator, PostgreSqlStringMemberTranslator>();

        return builder;
    }
}
