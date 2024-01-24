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
                .AddNoSql(builder => builder.UseSqlServer("Data Source=.;Database=TestDb;uid=sa;pwd=P@ssw0rd;TrustServerCertificate=true"))
                .BuildServiceProvider();
        }

        public static IServiceProvider Services { get; }
    }
}