using NoSql.Extensions;
using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace NoSql.Query;

public class SqlTranslatingExpressionVisitor(
    NoSqlOptions options,
    ISqlTranslatingExpressionVisitorFactory visitorFactory,
    ISqlTypeMappingSource typeMappingSource,
    ISqlExpressionTranslatorProvider translatorProvider,
    IDictionary<string, SqlExpression>? parameters) : ISqlTranslatingExpressionVisitor
{
    private static readonly IDictionary<string, SqlExpression> EmptyParameters = new Dictionary<string, SqlExpression>();

    protected readonly NoSqlOptions Options = options;
    protected readonly IDictionary<string, SqlExpression> Parameters = parameters ?? EmptyParameters;
    protected readonly ISqlTranslatingExpressionVisitorFactory VisitorFactory = visitorFactory;
    protected readonly ISqlTypeMappingSource TypeMappingSource = typeMappingSource;
    protected readonly ISqlExpressionTranslatorProvider TranslatorProvider = translatorProvider;

    private TypeMapping? _boolTypeMapping;
    private TypeMapping? _intTypeMapping;

    public SqlExpression Visit(Expression expression)
    {
        if (expression is ConstantExpression constantExpression)
            return VisitConstant(constantExpression);

        if (expression is BinaryExpression binaryExpression)
            return VisitBinary(binaryExpression);

        if (expression is UnaryExpression unaryExpression)
            return VisitUnary(unaryExpression);

        if (expression is MemberExpression memberExpression)
            return VisitMember(memberExpression);

        if (expression is NewExpression newExpression)
            return VisitNew(newExpression);

        if (expression is MemberInitExpression initExpression)
            return VisitMemberInit(initExpression);

        if (expression is ParameterExpression parameterExpression)
            return VisitParameter(parameterExpression);

        if (expression is MethodCallExpression methodCallExpression)
            return VisitMethodCall(methodCallExpression);

        if (expression is NewArrayExpression arrayExpression)
            return VisitNewArray(arrayExpression);

        if (expression is LambdaExpression lambdaExpression)
            return VisitLambda(lambdaExpression);

        ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);
        return null;
    }

    protected virtual SqlExpression VisitMember(MemberExpression node)
    {
        if (node.Expression != null && node.Expression.Type.IsNullableValueType())
        {
            if (node.Member.Name == nameof(Nullable<int>.Value))
            {
                return Visit(node.Expression);
            }
            else if (node.Member.Name == nameof(Nullable<int>.HasValue))
            {
                _boolTypeMapping ??= TypeMappingSource.FindMapping(typeof(bool));
                return new SqlUnaryExpression(_boolTypeMapping, ExpressionType.NotEqual, Visit(node.Expression));
            }
        }

        GetMemberAccessPath(node, out Expression? root, out MemberInfo[] members);

        if (root == null)
        {
            if (node.Member is PropertyInfo propertyInfo && propertyInfo.GetMethod != null && propertyInfo.GetMethod.Attributes.HasFlag(MethodAttributes.Static))
            {
                return new SqlConstantExpression(
                    node.Type,
                    TypeMappingSource.FindMapping(node.Type),
                    Expression.Lambda(node).Compile().DynamicInvoke());
            }
            else if (node.Member is FieldInfo fieldInfo && fieldInfo.Attributes.HasFlag(FieldAttributes.Static))
            {
                return new SqlConstantExpression(
                    node.Type,
                    TypeMappingSource.FindMapping(node.Type),
                    Expression.Lambda(node).Compile().DynamicInvoke());
            }
            else
            {
                ThrowHelper.ThrowTranslateException_ExpressionNotSupported(node);
            }
        }

        if (root is ConstantExpression)
        {
            var value = Expression.Lambda(node).Compile().DynamicInvoke();
            return new SqlConstantExpression(node.Type, TypeMappingSource.FindMapping(node.Type), value);
        }

        SqlExpression instance;
        string[] path;
        if (root is ParameterExpression parameter)
        {
            if (!Parameters.TryGetValue(parameter.Name!, out var value))
            {
                ThrowHelper.ThrowTranslateException_ParameterNotFound(parameter.Name!);
            }

            if (value is SqlTableExpression tableExpression)
            {
                var tableInfo = tableExpression.TableInfo;
                var columnInfo = tableInfo.GetMappedColumn(members[0].Name);
                instance = new SqlColumnExpression(columnInfo, tableExpression.Alias);
                if (members.Length == 1)
                {
                    path = [];
                }
                else
                {
                    path = members.Skip(1).Select(x => x.Name).ToArray();
                }
            }
            else if (value is SqlJsonArrayEachItemExpression)
            {
                path = members.Select(x => x.Name).ToArray();
                instance = value;
            }
            else
            {
                ThrowHelper.ThrowTranslateException_ExpressionNotExpected(nameof(SqlTableExpression));
                return null;

            }
        }
        else
        {
            var exp = Visit(root);
            if (exp is not SqlColumnExpression columnExpression)
            {
                ThrowHelper.ThrowTranslateException_ExpressionNotExpected(nameof(SqlColumnExpression));
                return null;
            }

            instance = columnExpression;
            path = members.Select(x => x.Name).ToArray();
        }

        if (path.Length == 0)
        {
            return instance;
        }

        return TranslatorProvider.TranslateMember(node.Member, instance)
            ?? new SqlJsonExtractExpression(node.Type, TypeMappingSource.FindMapping(node.Type), instance, path);
    }

    protected virtual SqlExpression VisitMemberInit(MemberInitExpression node)
    {
        //if (TryEvaluate(node, out var constantExpression))
        //{
        //    return constantExpression;
        //}

        return new SqlJsonObjectExpression(
            TypeMappingSource.FindMapping(node.Type),
            node.Bindings
                .Cast<MemberAssignment>()
                .ToDictionary(x => x.Member.Name, x => Visit(x.Expression)));
    }

    protected virtual SqlExpression VisitNew(NewExpression node)
    {
        //if (TryEvaluate(node, out var constantExpression))
        //{
        //    return constantExpression;
        //}

        Dictionary<string, SqlExpression> properties = new(node.Members!.Count);
        for (int i = 0; i < node.Members.Count; i++)
        {
            properties.Add(node.Members[i].Name, Visit(node.Arguments[i]));
        }
        return new SqlJsonObjectExpression(TypeMappingSource.FindMapping(node.Type), properties);
    }

    protected virtual SqlExpression VisitNewArray(NewArrayExpression node)
    {
        if (TryEvaluate(node, out var constantExpression))
        {
            return constantExpression;
        }

        ThrowHelper.ThrowTranslateException_CanNotEvaluated(node);
        return null;
    }

    protected virtual SqlExpression VisitBinary(BinaryExpression node)
    {
        var left = TryRemoveImplicitConvert(node.Left);
        var right = TryRemoveImplicitConvert(node.Right);

        if (left.Type != right.Type)
        {
            TryConvertConstant(ref left, ref right);
        }

        if (node.NodeType == ExpressionType.NotEqual ||
            node.NodeType == ExpressionType.Equal)
        {
            Expression? operand = null;

            if (node.Left is ConstantExpression constant && constant.Value == null)
            {
                operand = node.Right;
            }
            else if (node.Right is ConstantExpression constant2 && constant2.Value == null)
            {
                operand = node.Left;
            }

            if (operand != null)
            {
                _boolTypeMapping ??= TypeMappingSource.FindMapping(typeof(bool));
                return new SqlUnaryExpression(_boolTypeMapping, node.NodeType, Visit(operand));
            }
        }

        if (node.NodeType == ExpressionType.ArrayIndex)
        {
            var instance = Visit(left);
            var index = Visit(right);
            if (instance is SqlColumnExpression column)
            {
                var elementType = column.Type.GetElementType()!;
                return new SqlJsonExtractExpression(TypeMappingSource.FindMapping(elementType), column, new PathSegment(index));
            }
            else if (instance is SqlJsonExtractExpression extract)
            {
                var elementType = extract.Type.GetElementType()!;
                var newPath = new PathSegment[extract.Path.Length + 1];
                Array.Copy(extract.Path, newPath, extract.Path.Length);
                newPath[^1] = new PathSegment(index);
                return new SqlJsonExtractExpression(TypeMappingSource.FindMapping(elementType), extract.Column, newPath);
            }
            else
            {
                ThrowHelper.ThrowTranslateException_ExpressionNotExpected(nameof(SqlColumnExpression), nameof(SqlJsonExtractExpression));
            }
        }

        return new SqlBinaryExpression(
            node.Type,
            TypeMappingSource.FindMapping(node.Type),
            node.NodeType,
            Visit(left),
            Visit(right));

        static void TryConvertConstant(ref Expression left, ref Expression right)
        {
            if (right.Type == typeof(char) && left is ConstantExpression cl && cl.Type == typeof(int))
            {
                left = Expression.Constant((char)(ushort)cl.Value!, typeof(char));
            }
            else if (left.Type == typeof(char) && right is ConstantExpression cr && cr.Type == typeof(int))
            {
                right = Expression.Constant((char)(int)cr.Value!, typeof(char));
            }
        }
    }

    protected virtual SqlExpression VisitUnary(UnaryExpression node)
    {
        var operand = Visit(node.Operand);
        if (node.NodeType == ExpressionType.Convert)
        {
            if (node.Type.IsNullableValueType())
                return operand;
        }

        if (node.NodeType == ExpressionType.ConvertChecked ||
            node.NodeType == ExpressionType.Convert)
        {
            return new SqlCastExpression(TypeMappingSource.FindMapping(node.Type), operand);
        }

        if (node.NodeType == ExpressionType.ArrayLength)
        {
            _intTypeMapping ??= TypeMappingSource.FindMapping(typeof(int));
            return new SqlJsonArrayLengthExpression(_intTypeMapping, operand);
        }

        if (operand is SqlUnaryExpression innerExpression)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                if (innerExpression.OperatorType == ExpressionType.Equal)
                {
                    return new SqlUnaryExpression(
                        innerExpression.TypeMapping!,
                        ExpressionType.NotEqual,
                        innerExpression.Operand);
                }
                else if (innerExpression.OperatorType == ExpressionType.NotEqual)
                {
                    return new SqlUnaryExpression(
                        innerExpression.TypeMapping!,
                        ExpressionType.Equal,
                        innerExpression.Operand);
                }
                else if (innerExpression.OperatorType == ExpressionType.Not && innerExpression.Type == typeof(bool))
                {
                    return innerExpression.Operand;
                }
            }
            else if (node.NodeType == ExpressionType.Negate)
            {
                if (innerExpression.OperatorType == ExpressionType.Not)
                {
                    return innerExpression.Operand;
                }
            }
        }

        return new SqlUnaryExpression(
            node.Type,
            TypeMappingSource.FindMapping(node.Type),
            node.NodeType,
            operand);


    }

    protected virtual SqlExpression VisitConstant(ConstantExpression node)
    {
        return new SqlConstantExpression(node.Type, TypeMappingSource.FindMapping(node.Type), node.Value);
    }

    protected virtual SqlExpression VisitMethodCall(MethodCallExpression node)
    {
        var instance = node.Object == null ? null : Visit(node.Object);

        SqlExpression[] arguments = new SqlExpression[node.Arguments.Count];
        for (int i = 0; i < node.Arguments.Count; i++)
        {
            var argument = node.Arguments[i];
            arguments[i] = Visit(argument);
        }

        var expression = TranslatorProvider.TranslateMethod(node.Method, instance, arguments);

        if (expression == null)
        {
            ThrowHelper.ThrowTranslateException_MethodInfoNotSupported(node.Method);
        }

        return expression;
    }

    protected virtual SqlExpression VisitParameter(ParameterExpression node)
    {
        if (!Parameters.TryGetValue(node.Name!, out var parameter))
        {
            ThrowHelper.ThrowTranslateException_ParameterNotFound(node.Name!);
        }

        return parameter;
    }

    protected virtual SqlExpression VisitLambda(LambdaExpression node)
    {
        if (node.Parameters.Count != 1)
        {
            ThrowHelper.ThrowTranslateException_LambdaExpressionParameterNotSupported();
        }

        var parameterType = node.Parameters[0].Type;

        return VisitorFactory
            .Create(
                node.Parameters[0].Name!,
                new SqlJsonArrayEachItemExpression(TypeMappingSource.FindMapping(parameterType)))
            .Visit(node.Body);
    }

    private bool TryEvaluate(Expression expression, [NotNullWhen(true)] out SqlConstantExpression? result)
    {
        if (EvaluatableExpressionVisitor.CanEvaluate(expression))
        {
            result = new SqlConstantExpression(expression.Type, TypeMappingSource.FindMapping(expression.Type), Expression.Lambda(expression).Compile().DynamicInvoke());
            return true;
        }
        result = null;
        return false;
    }

    private static Expression TryRemoveImplicitConvert(Expression expression)
    {
        if (expression is UnaryExpression unaryExpression
            && (unaryExpression.NodeType == ExpressionType.Convert
                || unaryExpression.NodeType == ExpressionType.ConvertChecked))
        {
            var innerType = unaryExpression.Operand.Type.UnwrapNullableType();
            if (innerType.IsEnum)
            {
                innerType = Enum.GetUnderlyingType(innerType);
            }

            var convertedType = expression.Type.UnwrapNullableType();

            if (innerType == convertedType
                || convertedType == typeof(int)
                    && (innerType == typeof(byte)
                        || innerType == typeof(sbyte)
                        || innerType == typeof(char)
                        || innerType == typeof(short)
                        || innerType == typeof(ushort)))
            {
                return TryRemoveImplicitConvert(unaryExpression.Operand);
            }
        }

        return expression;
    }

    private static void GetMemberAccessPath(MemberExpression memberExpression, out Expression? root, out MemberInfo[] path)
    {
        var exp = memberExpression.Expression;
        List<MemberInfo> members = [memberExpression.Member];
        for (; ; )
        {
            if (exp is MemberExpression member)
            {
                members.Add(member.Member);
                exp = member.Expression;
            }
            else
            {
                members.Reverse();
                root = exp;
                path = [.. members];
                return;
            }
        }
    }

    private class EvaluatableExpressionVisitor : ExpressionVisitor
    {
        public static bool CanEvaluate(Expression expression)
        {
            var visitor = new EvaluatableExpressionVisitor();
            visitor.Visit(expression);
            return visitor._canEvaluate;
        }

        private bool _canEvaluate = true;

        public override Expression? Visit(Expression? node)
        {
            if (!_canEvaluate)
            {
                return node;
            }
            return base.Visit(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            _canEvaluate = false;
            return node;
        }
    }
}