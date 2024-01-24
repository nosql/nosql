using Microsoft.Extensions.DependencyInjection;
using static NoSql.Test.DB;

namespace NoSql.Test;

public class DB : DB<SqlServerServiceProviderFactory>
{
    public class SqlServerServiceProviderFactory : IServiceProviderFactory
    {
        static SqlServerServiceProviderFactory()
        {
            Services = new ServiceCollection()
                .AddNoSql(builder => builder.UsePostgreSql("Host=localhost;Port=5433;Database=TestDb;User Id=postgres;Password=P@ssw0rd"))
                .BuildServiceProvider();
        }

        public static IServiceProvider Services { get; }
    }
}

