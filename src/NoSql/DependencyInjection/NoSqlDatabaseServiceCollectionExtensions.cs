using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoSql.Query;
using NoSql.Query.Translators;
using NoSql.Scaffolding;
using NoSql.Storage;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NoSql;

public static class NoSqlDatabaseServiceCollectionExtensions
{
    public static IServiceCollection AddNoSql(this IServiceCollection services, Action<NoSqlBuilder> builderAction)
    {
        return AddNoSql(services, ServiceLifetime.Scoped, builderAction);
    }

    public static IServiceCollection AddNoSql<TDatabase>(this IServiceCollection services, Action<NoSqlBuilder> builderAction)
        where TDatabase : NoSqlDatabase
    {
        return AddNoSql(services, ServiceLifetime.Scoped, builderAction);
    }

    public static IServiceCollection AddNoSql(this IServiceCollection services, ServiceLifetime lifetime, Action<NoSqlBuilder> builderAction)
    {
        services.TryAddNoSqlCore(lifetime);
        NoSqlBuilder builder = new(services, lifetime);
        builderAction(builder);

        services.AddNoSqlTranslator();
        services.AddSingleton(new NoSqlOptions(builder.SerializerOptions));
        services.TryAdd(new ServiceDescriptor(typeof(NoSqlDatabase), typeof(NoSqlDatabase), lifetime));
        services.TryAdd(new ServiceDescriptor(typeof(NoSqlDependencies), typeof(NoSqlDependencies), lifetime));
        return services;
    }

    public static IServiceCollection AddNoSql<TDatabase>(this IServiceCollection services, ServiceLifetime lifetime, Action<NoSqlBuilder<TDatabase>> builderAction)
        where TDatabase : NoSqlDatabase
    {
        ServiceCollection serviceMap = new();
        serviceMap.TryAddNoSqlCore(lifetime);

        NoSqlBuilder<TDatabase> builder = new(serviceMap, lifetime);
        builderAction(builder);
        serviceMap.AddSingleton(new NoSqlOptions(builder.SerializerOptions));
        serviceMap.AddNoSqlTranslator();

        var serviceProvider = serviceMap.BuildServiceProvider();

        services.Add(new ServiceDescriptor(typeof(TDatabase), typeof(TDatabase), lifetime));
        services.Add(new ServiceDescriptor(typeof(SqlDatabaseDependencies<TDatabase>), p => new SqlDatabaseDependencies<TDatabase>(
            serviceProvider.GetRequiredService<INoSqlDbConnection>(),
            serviceProvider.GetRequiredService<ISqlGeneratorFactory>(),
            serviceProvider.GetRequiredService<ISqlTypeMappingSource>(),
            serviceProvider.GetRequiredService<IDatabaseFactory>(),
            serviceProvider.GetRequiredService<ITypeInfoResolver>(),
            serviceProvider.GetRequiredService<ISqlTranslatingExpressionVisitorFactory>(),
            serviceProvider.GetRequiredService<ISqlExpressionFactory>()
        ), lifetime));
        return services;
    }

    private static IServiceCollection TryAddNoSqlCore(this IServiceCollection services, ServiceLifetime lifetime)
    {
        services.TryAdd(new ServiceDescriptor(typeof(IDatabaseFactory), typeof(RelationalDatabaseFactory), lifetime));
        services.TryAddSingleton<IDatabaseFactory, RelationalDatabaseFactory>();

        services.TryAddSingleton<ISqlExpressionFactory, SqlExpressionFactory>();
        services.TryAddSingleton<ITypeInfoResolver, TypeInfoReflectionResolver>();
        services.TryAddSingleton<ISqlGeneratorFactory, SqlGeneratorFactory>();
        services.TryAddSingleton<ISqlTranslatingExpressionVisitorFactory, SqlTranslatingExpressionVisitorFactory>();
        services.TryAddSingleton<ISqlExpressionTranslatorProvider, SqlExpressionTranslatorProvider>();

        return services;
    }

    private static IServiceCollection AddNoSqlTranslator(this IServiceCollection services)
    {
        services.AddSingleton<IMethodCallTranslator, ObjectMethodTranslator>();
        services.AddSingleton<IMethodCallTranslator, StringMethodTranslator>();
        services.AddSingleton<IMethodCallTranslator, EnumerableMethodTranslator>();
        return services;
    }
}

public class NoSqlOptions
{
    public NoSqlOptions(JsonSerializerOptions? serializerOptions = null)
    {
        SerializerOptions = serializerOptions ?? NoSqlBuilder.DefaultSerializerOptions;
    }

    public JsonSerializerOptions SerializerOptions { get; }
}

public class NoSqlBuilder
{
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    internal NoSqlBuilder(IServiceCollection services, ServiceLifetime lifetime)
    {
        Services = services;
        SerializerOptions = DefaultSerializerOptions;
        Lifetime = lifetime;
    }

    public ServiceLifetime Lifetime { get; }
    public IServiceCollection Services { get; }

    public JsonSerializerOptions SerializerOptions { get; set; }
}

public class NoSqlBuilder<TDatabase> : NoSqlBuilder
{
    internal NoSqlBuilder(IServiceCollection services, ServiceLifetime lifetime) : base(services, lifetime)
    {
    }
}
