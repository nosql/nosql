using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Reflection;

namespace NoSql.Query.Translators;

public sealed class StringMethodTranslator : IMethodCallTranslator
{
    private static readonly MethodInfo IsNullOrEmptyMethodInfo
        = typeof(string).GetRuntimeMethod(nameof(string.IsNullOrEmpty), new[] { typeof(string) })!;

    private static readonly MethodInfo ContainsMethodInfo
        = typeof(string).GetRuntimeMethod(nameof(string.Contains), new[] { typeof(string) })!;

    private static readonly MethodInfo StartsWithMethodInfo
        = typeof(string).GetRuntimeMethod(nameof(string.StartsWith), new[] { typeof(string) })!;

    private static readonly MethodInfo EndsWithMethodInfo
        = typeof(string).GetRuntimeMethod(nameof(string.EndsWith), new[] { typeof(string) })!;

    private static readonly MethodInfo ConcatMethodInfoTwoArgs
        = typeof(string).GetRuntimeMethod(nameof(string.Concat), new[] { typeof(string), typeof(string) })!;

    private static readonly MethodInfo ConcatMethodInfoThreeArgs
        = typeof(string).GetRuntimeMethod(nameof(string.Concat), new[] { typeof(string), typeof(string), typeof(string) })!;

    private static readonly MethodInfo ConcatMethodInfoFourArgs
        = typeof(string).GetRuntimeMethod(
            nameof(string.Concat), new[] { typeof(string), typeof(string), typeof(string), typeof(string) })!;

    private readonly ISqlTypeMappingSource _mappingSource;

    public StringMethodTranslator(ISqlTypeMappingSource mappingSource)
    {
        _mappingSource = mappingSource;
    }

    private TypeMapping? _boolTypeMapping;

    public SqlExpression? Translate(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments)
    {
        _boolTypeMapping ??= _mappingSource.FindMapping(typeof(bool));
        if (method == ContainsMethodInfo)
        {
            if (instance == null || arguments.Length != 1)
                return null;

            if (arguments[0] is not SqlConstantExpression constant || constant.Value is not string match)
                return null;

            return new SqlLikeExpression(_boolTypeMapping, instance, $"%{match}%");
        }
        else if (method == StartsWithMethodInfo)
        {
            if (instance == null || arguments.Length != 1)
                return null;

            if (arguments[0] is not SqlConstantExpression constant || constant.Value is not string match)
                return null;

            return new SqlLikeExpression(_boolTypeMapping, instance, $"{match}%");
        }
        else if (method == EndsWithMethodInfo)
        {
            if (instance == null || arguments.Length != 1)
                return null;

            if (arguments[0] is not SqlConstantExpression constant || constant.Value is not string match)
                return null;

            return new SqlLikeExpression(_boolTypeMapping, instance, $"%{match}");
        }

        return null;
    }

}