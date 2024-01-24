using Microsoft.Extensions.DependencyInjection;
using NoSql.Query;
using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Linq.Expressions;

namespace NoSql.Test;

public class DB<TFactory> where TFactory : IServiceProviderFactory
{
    private static NoSqlDatabase Database;
    private static ISqlGeneratorFactory GeneratorFactory;
    private static ISqlTypeMappingSource TypeMappingSource;
    private static ISqlTranslatingExpressionVisitorFactory ExpressionTranslator;
    private static bool _init;

    public static NoSqlCollection<T> Table<T>() => TableCache<T>.Instance;

    public static string Generate(SqlExpression expression)
    {
        Initizaile();
        return GeneratorFactory.Create().Generate(expression);
    }

    public static string Generate(Expression expression)
    {
        Initizaile();
        return GeneratorFactory.Create().Generate(ExpressionTranslator.Create().Visit(expression));
    }

    public static NoSqlCollection<T> Table<T>(string? name = null)
    {
        Initizaile();
        return Database.Collection<T>(name);
    }

    public static SqlConstantExpression Constant<T>(T? value)
    {
        return new SqlConstantExpression(TypeMappingSource.FindMapping(typeof(T)), value);
    }

    public static void Initizaile()
    {
        if (!_init)
        {
            Database = TFactory.Services.GetRequiredService<NoSqlDatabase>();
            GeneratorFactory = TFactory.Services.GetRequiredService<ISqlGeneratorFactory>();
            TypeMappingSource = TFactory.Services.GetRequiredService<ISqlTypeMappingSource>();
            ExpressionTranslator = TFactory.Services.GetRequiredService<ISqlTranslatingExpressionVisitorFactory>();


            Database.Collection<MappedModel>().Drop();
            Database.Collection<MappedModel>().EnsureCreated();
            Database.Collection<MappedModel>().Insert(new MappedModel
            {
                Id = 1,
                Name = "a"
            });
            Database.Collection<MappedModel>().Insert(new MappedModel
            {
                Id = 2,
                Name = "b"
            });

            Database.Collection<InsertDeleteTestObject>().Drop();
            Database.Collection<InsertDeleteTestObject>().EnsureCreated();

            Database.Collection<AggregateTestObject>().Drop();
            Database.Collection<AggregateTestObject>().EnsureCreated();
            Database.Collection<AggregateTestObject>().Insert(new AggregateTestObject(1, 1));
            Database.Collection<AggregateTestObject>().Insert(new AggregateTestObject(2, 3));
            Database.Collection<AggregateTestObject>().Insert(new AggregateTestObject(3, 3));
            Database.Collection<AggregateTestObject>().Insert(new AggregateTestObject(4, 4));

            Database.Collection<PrimitiveValueQueryTestObject>().Drop();
            Database.Collection<PrimitiveValueQueryTestObject>().EnsureCreated();
            Database.Collection<PrimitiveValueQueryTestObject>().Insert(PrimitiveValueQueryTestObject.Create());

            Database.Collection<PrimitiveValueUpdateTestObject>().Drop();
            Database.Collection<PrimitiveValueUpdateTestObject>().EnsureCreated();
            Database.Collection<PrimitiveValueUpdateTestObject>().Insert(PrimitiveValueUpdateTestObject.Create());

            Database.Collection<JsonValueQueryTestObject>().Drop();
            Database.Collection<JsonValueQueryTestObject>().EnsureCreated();
            Database.Collection<JsonValueQueryTestObject>().Insert(JsonValueQueryTestObject.Create());

            Database.Collection<JsonValueUpdateTestObject>().Drop();
            Database.Collection<JsonValueUpdateTestObject>().EnsureCreated();
            Database.Collection<JsonValueUpdateTestObject>().Insert(JsonValueUpdateTestObject.Create());

            _init = true;
        }
    }

    private static class TableCache<T>
    {
        public static readonly NoSqlCollection<T> Instance;

        static TableCache()
        {
            Initizaile();
            Instance = Database!.Collection<T>();
        }
    }
}

public interface IServiceProviderFactory
{
    abstract static IServiceProvider Services { get; }
}