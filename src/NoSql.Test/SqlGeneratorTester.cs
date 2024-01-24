using NoSql.Query;
using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text.Json;

namespace NoSql;

public class SqlGeneratorTester
{
    private static readonly ISqlTranslatingExpressionVisitorFactory TranslatingVisitor = new SqlTranslatingExpressionVisitorFactory(new NoSqlOptions(), TestTypeMappingSource.Default, null!);
    public static readonly ITypeInfoResolver TypeInfoResolver = new TypeInfoReflectionResolver(TestTypeMappingSource.Default);

    public static string Generate(SqlExpression expression)
    {
        return new SqlGenerator().Generate(expression);
    }

    public static string Generate<TTable>(LambdaExpression expression)
    {
        var typeInfo = TypeInfoResolver.GetTypeInfo(typeof(TTable));
        var exp = TranslatingVisitor
            .Create(expression.Parameters[0].Name!, new SqlTableExpression(typeInfo))
            .Visit(expression.Body);
        return new SqlGenerator().Generate(exp);
    }

    public static SqlConstantExpression Constant<T>(T value)
    {
        return new SqlConstantExpression(TestTypeMappingSource.Default.FindMapping(typeof(T)), value);
    }
    public static SqlConstantExpression Constant(object? value)
    {
        var type = value?.GetType() ?? typeof(object);
        return new SqlConstantExpression(TestTypeMappingSource.Default.FindMapping(type), value);
    }


    private class TestTypeMappingSource : ISqlTypeMappingSource
    {
        public static readonly TestTypeMappingSource Default = new();
        public TypeMapping FindMapping(Type type, bool jsonType = false) => new TestTypeMapping(type, type.Name);
    }

    private sealed class TestTypeMapping : TypeMapping
    {
        public TestTypeMapping(Type clrType, string storeType) : base(clrType, storeType)
        {
            IsJsonType = true;
        }

        public override bool IsJsonType { get; }

        public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
        {
            throw new NotImplementedException();
        }

        protected override string GenerateNonNullSqlLiteral(object value)
        {
            if (value is string s)
                return $"'{s}'";

            if (value is char c)
                return $"'{c}'";

            if (value is bool b)
                return b ? "1" : "0";

            if (ClrType.IsValueType)
                return value.ToString()!;

            return "'" + JsonSerializer.Serialize(value) + "'";
        }
    }
}