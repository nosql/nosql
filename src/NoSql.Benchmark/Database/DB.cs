// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using NoSql;
using NoSql.Benchmark;
using NoSql.Test;

public static class DB
{
    public static readonly SqlServerDatabase SqlServer;
    public static readonly SqliteDatabase Sqlite;
    public static readonly SqliteWithJsonObjectDatabase SqliteWithJsonObject;

    public static readonly PostgreSqlDatabase PostgreSql;

    static DB()
    {
        PostgreSql = new ServiceCollection().AddNoSql<PostgreSqlDatabase>(builder =>
        {
            builder.UsePostgreSql("Host=localhost;Port=5433;Database=BenchmarkDb;User Id=postgres;Password=P@ssw0rd");
        }).BuildServiceProvider().GetRequiredService<PostgreSqlDatabase>();

        Sqlite = new ServiceCollection().AddNoSql<SqliteDatabase>(builder =>
        {
            builder.UseSqlite("Data Source=C:\\Dev\\gitee\\staiia\\nosql\\src\\NoSql.Benchmark\\bin\\Debug\\net7.0\\Benchmark.sqlite");
        }).BuildServiceProvider().GetRequiredService<SqliteDatabase>();

        SqlServer = new ServiceCollection().AddNoSql<SqlServerDatabase>(builder =>
        {
            builder.UseSqlServer("Data Source=.;Database=BenchmarkDb;uid=sa;pwd=P@ssw0rd;TrustServerCertificate=true");
        }).BuildServiceProvider().GetRequiredService<SqlServerDatabase>();
    }

    public static void InitData()
    {
        PostgreSql.Collection<PrimitiveValueQueryTestObject>().EnsureCreated();
        if (PostgreSql.Collection<BenchmarkPrimitiveValueObject>().EnsureCreated() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 10000; i++)
            {
                PostgreSql.Collection<BenchmarkPrimitiveValueObject>().Insert(BenchmarkPrimitiveValueObject.Create(i));
            }
        }
        if (PostgreSql.Collection<BenchmarkJsonValueObject>().EnsureCreated() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 10000; i++)
            {
                PostgreSql.Collection<BenchmarkJsonValueObject>().Insert(BenchmarkJsonValueObject.Create(i));
            }
        }

        Sqlite.Collection<PrimitiveValueQueryTestObject>().EnsureCreated();
        if (Sqlite.Collection<BenchmarkPrimitiveValueObject>().EnsureCreated() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 10000; i++)
            {
                Sqlite.Collection<BenchmarkPrimitiveValueObject>().Insert(BenchmarkPrimitiveValueObject.Create(i));
            }
        }
        if (Sqlite.Collection<BenchmarkJsonValueObject>().EnsureCreated() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 10000; i++)
            {
                Sqlite.Collection<BenchmarkJsonValueObject>().Insert(BenchmarkJsonValueObject.Create(i));
            }
        }

        SqlServer.Collection<PrimitiveValueQueryTestObject>().EnsureCreated();
        if (SqlServer.Collection<BenchmarkPrimitiveValueObject>().EnsureCreated() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 10000; i++)
            {
                SqlServer.Collection<BenchmarkPrimitiveValueObject>().Insert(BenchmarkPrimitiveValueObject.Create(i));
            }
        }
        if (SqlServer.Collection<BenchmarkJsonValueObject>().EnsureCreated() == NoSql.Scaffolding.TableCreateResult.Created)
        {
            for (int i = 0; i < 10000; i++)
            {
                SqlServer.Collection<BenchmarkJsonValueObject>().Insert(BenchmarkJsonValueObject.Create(i));
            }
        }
    }

    public static NoSqlDatabase GetDatabase(DatabaseKind kind) => kind switch
    {
        DatabaseKind.Sqlite => Sqlite,
        DatabaseKind.SqlServer => SqlServer,
        _ => PostgreSql,
    };

}
