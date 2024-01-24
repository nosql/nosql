using Microsoft.Extensions.DependencyInjection;
using static NoSql.Test.DB;

namespace NoSql.Test;

public class DB : DB<SqliteServiceProviderFactory>
{
    public class SqliteServiceProviderFactory : IServiceProviderFactory
    {
        static SqliteServiceProviderFactory()
        {
            Services = new ServiceCollection()
                .AddNoSql(builder =>
                {
                    builder.UseArangoDb(options =>
                    {
                        options.Url = "http://localhost:8529";
                        options.Database = "TestDb";
                        options.UserName = "root";
                        options.Password = "P@ssw0rd";
                    });
                })
                .BuildServiceProvider();
        }

        public static IServiceProvider Services { get; }
    }
}

