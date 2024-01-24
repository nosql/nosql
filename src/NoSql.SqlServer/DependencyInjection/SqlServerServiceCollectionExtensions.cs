using Microsoft.Extensions.DependencyInjection;
using NoSql.Query;
using NoSql.Query.Translators;
using NoSql.Scaffolding;
using NoSql.SqlServer.Query;
using NoSql.SqlServer.Query.Translators;
using NoSql.SqlServer.Scaffolding;
using NoSql.SqlServer.Storage;
using NoSql.Storage;

namespace NoSql;

public static class SqlServerServiceCollectionExtensions
{
    public static NoSqlBuilder UseSqlServer(this NoSqlBuilder builder, string connectionString)
    {
        builder.Services.Add(new ServiceDescriptor(typeof(INoSqlDbConnection), p => new SqlServerDbConnection(connectionString), builder.Lifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(IDatabaseFactory), typeof(SqlServerDatabaseFactory), builder.Lifetime));

        builder.Services.AddSingleton<ISqlExpressionFactory, SqlServerSqlExpressionFactory>();
        builder.Services.AddSingleton<ISqlGeneratorFactory, SqlServerSqlGeneratorFactory>();
        builder.Services.AddSingleton<ISqlTypeMappingSource, SqlServerTypeMappingSource>();

        builder.Services.AddSingleton<IMemberTranslator, SqlServerDateTimeMemberTranslator>();
        builder.Services.AddSingleton<IMemberTranslator, SqlServerStringMemberTranslator>();

        return builder;
    }
}
