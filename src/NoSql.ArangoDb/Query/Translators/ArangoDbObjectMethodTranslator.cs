using NoSql.ArangoDb.Storage;
using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using System.Reflection;

namespace NoSql.ArangoDb.Query.Translators;

public sealed class ArangoDbObjectMethodTranslator : IMethodCallTranslator
{
    public SqlExpression? Translate(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments)
    {
        if (method.Name == nameof(ToString) && instance != null && arguments.Length == 0)
        {
            if (instance.Type == typeof(string))
            {
                return instance;
            }

            return new SqlFunctionExpression(ArangoDbTypeMappingSource.StringType, "TO_STRING", instance);
        }

        return null;
    }
}