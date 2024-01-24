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
                    builder.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.db")}");
                })
                .BuildServiceProvider();
        }

        public static IServiceProvider Services { get; }
    }
}

