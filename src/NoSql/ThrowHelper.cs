using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace NoSql;

internal sealed class ThrowHelper
{
    [DoesNotReturn]
    public static void ThrowArgumentNullException(string argumentName)
    {
        throw new ArgumentNullException(argumentName, $"Argument '{argumentName}' cannot be null.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_CannotGetElementTypeException(Type type)
    {
        throw new NoSqlTranslateException($"Cannot get element type from '{type.FullName}'.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_NotFoundPrimaryKey(NoSqlTypeInfo typeInfo)
    {
        throw new NoSqlTranslateException($"Cannot found primary key from type '{typeInfo.Name}'.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_PrimaryKeyNotMatched(NoSqlTypeInfo typeInfo)
    {
        throw new NoSqlTranslateException($"The type '{typeInfo.Name}' primary key cannot be matched.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_ColumnTypeNotExpected(string columnName)
    {
        throw new NoSqlTranslateException($"The type of column '{columnName}' not expected.");
    }
    [DoesNotReturn]
    public static void ThrowTranslateException_ExpressionNotSupported(Expression expression)
    {
        throw new NoSqlTranslateException($"Expression '{expression.NodeType}' not supported.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_ExpressionNotSupported(SqlExpression expression)
    {
        throw new NoSqlTranslateException($"SqlExpression '{expression.GetType()}' not supported.");
    }


    [DoesNotReturn]
    public static void ThrowTranslateException_ExpressionNotExpected(string expectedExpression)
    {
        throw new NoSqlTranslateException($"Expression is not '{expectedExpression}'.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_ExpressionNotExpected(string expectedExpression1, string expectedExpression2)
    {
        throw new NoSqlTranslateException($"Expression is not '{expectedExpression1}' or '{expectedExpression2}'.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_CanNotEvaluated(Expression expression)
    {
        throw new NoSqlTranslateException($"Expression '{expression.NodeType}' can not Evaluated.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_MethodInfoNotSupported(MethodInfo method)
    {
        throw new NoSqlTranslateException($"MethodInfo '{method.DeclaringType?.Name}.{method.Name}' not supported.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_ParameterNotFound(string parameterName)
    {
        throw new NoSqlTranslateException($"Parameter '{parameterName}' not found.");
    }

    [DoesNotReturn]
    public static void ThrowTranslateException_LambdaExpressionParameterNotSupported()
    {
        throw new NoSqlTranslateException("Lambda expression parameters count not be greater than 1.");
    }

    [DoesNotReturn]
    public static void ThrowTypeException_ConstructorNotFound(Type type)
    {
        throw new NoSqlTranslateException($"Type '{type.Name}' not found no parameters constructor.");
    }

    [DoesNotReturn]
    public static void ThrowTypeException_ColumnNotFound(string table, string column)
    {
        throw new NoSqlTranslateException($"Column '{column}' not found in table '{table}'.");
    }

    [DoesNotReturn]
    public static void ThrowParseException_ParseError(string message, int position)
    {
        throw new NoSqlSyntaxParseException(message, position);
    }

    [DoesNotReturn]
    public static void ThrowParseException_ParseError(int position)
    {
        throw new NoSqlSyntaxParseException("Syntax error.", position);
    }
}

