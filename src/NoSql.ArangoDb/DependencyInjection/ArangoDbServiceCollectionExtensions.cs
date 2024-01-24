using Microsoft.Extensions.DependencyInjection;
using NoSql.ArangoDb;
using NoSql.ArangoDb.Query;
using NoSql.ArangoDb.Query.Translators;
using NoSql.ArangoDb.Scaffolding;
using NoSql.ArangoDb.Storage;
using NoSql.Query;
using NoSql.Query.Translators;
using NoSql.Scaffolding;
using NoSql.Storage;
using System.Text;

namespace NoSql;

public static class ArangoDbServiceCollectionExtensions
{
    public static NoSqlBuilder UseArangoDb(this NoSqlBuilder builder, Action<ArangoOptions> optionsAction)
    {
        ArangoOptions options = new();
        optionsAction(options);

        string name = "nosql_arangodb_" + options.Database;
        string url = options.Url.EndsWith('/') ? options.Url[..^1] : options.Url;

        builder.Services.AddHttpClient(name, client =>
        {
            client.BaseAddress = new Uri($"{url}/_db/{options.Database}/_api/");
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(options.UserName + ":" + options.Password)));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromMinutes(5);
        });

        builder.Services.AddSingleton(options);
        builder.Services.Add(new ServiceDescriptor(typeof(NoSqlDatabase), typeof(ArangoDatabase), builder.Lifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(ArangoDbConnection), typeof(ArangoDbConnection), builder.Lifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(INoSqlDbConnection), typeof(ArangoDbConnection), builder.Lifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(IDatabaseFactory), typeof(ArangoDatabaseFactory), builder.Lifetime));

        builder.Services.AddSingleton<ISqlGeneratorFactory, AqlGeneratorFactory>();
        builder.Services.AddSingleton<ISqlTypeMappingSource, ArangoDbTypeMappingSource>();
        builder.Services.AddSingleton<ISqlExpressionFactory, AqlExpressionFactory>();

        builder.Services.AddSingleton<IMethodCallTranslator, ArangoDbObjectMethodTranslator>();
        builder.Services.AddSingleton<IMemberTranslator, ArangoDbStringMemberTranslator>();
        builder.Services.AddSingleton<IMemberTranslator, ArangoDbDateTimeFunctionTranslator>();
        builder.Services.AddSingleton<IMethodCallTranslator, ArangoDbEnumerableMethodTranslator>();

        return builder;
    }
}

public class ArangoOptions
{
    public string Url { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}
