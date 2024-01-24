using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Reflection;

namespace NoSql.Query.Translators;

public sealed class ObjectMethodTranslator : IMethodCallTranslator
{
    private readonly ISqlTypeMappingSource _mappingSource;

    public ObjectMethodTranslator(ISqlTypeMappingSource mappingSource)
    {
        _mappingSource = mappingSource;
    }

    public SqlExpression? Translate(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments)
    {
        if (method.Name == nameof(ToString) && instance != null && arguments.Length == 0)
        {
            if(instance.Type == typeof(string))
            {
                return instance;
            }

            return new SqlCastExpression(typeof(string), _mappingSource.FindMapping(typeof(string)), instance);
        }

        return null;
    }
}