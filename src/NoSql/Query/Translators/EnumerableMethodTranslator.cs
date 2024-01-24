using NoSql.Extensions;
using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Reflection;

namespace NoSql.Query.Translators;

public class EnumerableMethodTranslator : IMethodCallTranslator
{
    private static readonly Type EnumerableType = typeof(Enumerable);

    private readonly ISqlTypeMappingSource _mappingSource;
    private TypeMapping? _boolType;
    private TypeMapping? _intType;
    private TypeMapping? _doubleType;

    public EnumerableMethodTranslator(ISqlTypeMappingSource mappingSource)
    {
        _mappingSource = mappingSource;
    }

    public SqlExpression? Translate(MethodInfo method, SqlExpression? instance, SqlExpression[] arguments)
    {
        if (instance != null)
        {
            return null;
        }

        if (method.DeclaringType != EnumerableType)
        {
            return null;
        }

        if (method.IsGenericMethod)
        {
            method = method.GetGenericMethodDefinition();
        }

        _boolType ??= _mappingSource.FindMapping(typeof(bool));
        _intType ??= _mappingSource.FindMapping(typeof(int));

        if (method.GetParameters().Length != arguments.Length)
        {
            return null;
        }

        if (method.Name == nameof(Enumerable.Any))
        {
            return new SqlExistsExpression(_boolType,
                        new SqlSelectExpression(
                            new SqlFragmentExpression("1"),
                            new SqlJsonArrayEachExpression(arguments[0]),
                            arguments.Length == 1 ? null : arguments[1]));
        }
        else if (method.Name == nameof(Enumerable.All))
        {
            var predicate = new SqlUnaryExpression(_boolType, System.Linq.Expressions.ExpressionType.Not, arguments[1]);
            return new SqlExistsExpression(
                        _boolType,
                        new SqlSelectExpression(
                            new SqlFragmentExpression("1"),
                            new SqlJsonArrayEachExpression(arguments[0]),
                            predicate),
                        true);
        }
        else if (method.Name == nameof(Enumerable.Min))
        {
            return CreateAggregateExpression(AggregationType.Min, arguments[0], arguments.Length == 1 ? null : arguments[1]);
        }
        else if (method.Name == nameof(Enumerable.Max))
        {
            return CreateAggregateExpression(AggregationType.Max, arguments[0], arguments.Length == 1 ? null : arguments[1]);
        }
        else if (method.Name == nameof(Enumerable.Sum))
        {
            return CreateAggregateExpression(AggregationType.Sum, arguments[0], arguments.Length == 1 ? null : arguments[1]);
        }
        else if (method.Name == nameof(Enumerable.Average))
        {
            return CreateAggregateExpression(AggregationType.Average, arguments[0], arguments.Length == 1 ? null : arguments[1]);
        }
        else if (method.Name == nameof(Enumerable.Count))
        {
            return CreateCountExpression(arguments[0], arguments.Length == 1 ? null : arguments[1]);
        }
        else if (method.Name == nameof(Enumerable.Select))
        {
            var source = arguments[0];
            var projection = new SqlProjectionExpression(arguments[1], "p0");

            // Consolidate where().Select()
            if (SqlExpressionHelper.CanConsolidateProjectionOrPredicate(source))
            {
                if (source is SqlJsonBuildArrayExpression subquery)
                    return new SqlJsonBuildArrayExpression(subquery.Subquery.UpdateProjection(projection));
            }

            return new SqlJsonBuildArrayExpression(new SqlSelectExpression(projection, new SqlJsonArrayEachExpression(source)));
        }
        else if (method.Name == nameof(Enumerable.Where))
        {
            var source = arguments[0];
            var predicate = arguments[1];

            // Consolidate where().Where()
            if (SqlExpressionHelper.CanConsolidateProjectionOrPredicate(source))
            {
                if (source is SqlJsonBuildArrayExpression subquery)
                    return new SqlJsonBuildArrayExpression(subquery.Subquery.UpdatePredicate(predicate));
            }

            return new SqlJsonBuildArrayExpression(new SqlSelectExpression(
                            new SqlProjectionExpression(new SqlJsonArrayEachItemExpression(source.TypeMapping!), "p0"),
                            new SqlJsonArrayEachExpression(source),
                            predicate));
        }
        else if (
            method.Name == nameof(Enumerable.OrderBy) ||
            method.Name == nameof(Enumerable.OrderByDescending))
        {
            if (arguments.Length != 2)
            {
                return null;
            }

            var source = arguments[0];
            var ordering = new SqlOrderingExpression(arguments[1], method.Name == nameof(Enumerable.OrderByDescending));
            if (source is SqlJsonBuildArrayExpression subquery)
            {
                return new SqlJsonBuildArrayExpression(subquery.Subquery.UpdateOrdering(ordering));
            }
            else
            {
                return new SqlJsonBuildArrayExpression(new SqlSelectExpression(
                                new SqlProjectionExpression(new SqlJsonArrayEachItemExpression(source.TypeMapping!), "p0"),
                                new SqlJsonArrayEachExpression(source),
                                null,
                                new SqlOrderingExpression[] { ordering }));
            }
        }
        else if (method.Name == nameof(Enumerable.ElementAt))
        {
            if (arguments[0] is SqlColumnExpression column)
            {
                return column.Extract(GetElementTypeMapping(column.Type), new PathSegment(arguments[1]));
            }
            else if (arguments[0] is SqlJsonExtractExpression extractExpression)
            {
                return extractExpression.Extract(GetElementTypeMapping(extractExpression.Type), new PathSegment(arguments[1]));
            }
            else
            {
                return null;
            }
        }
        else if (method.Name == nameof(Enumerable.Take) || method.Name == nameof(Enumerable.Skip))
        {
            if (arguments.Length != 2)
            {
                return null;
            }

            if (arguments[1] is not SqlConstantExpression constantExpression || constantExpression.Type != typeof(int))
            {
                return null;
            }

            int? limit, offset;
            if (method.Name == nameof(Enumerable.Take))
            {
                limit = (int)constantExpression.Value!;
                offset = null;
            }
            else
            {
                offset = (int)constantExpression.Value!;
                limit = null;
            }
            var source = arguments[0];
            if (source is SqlJsonBuildArrayExpression subquery)
            {
                return new SqlJsonBuildArrayExpression(subquery.Subquery.UpdateLimitOffset(limit, offset));
            }
            else
            {
                return new SqlJsonBuildArrayExpression(new SqlSelectExpression(
                                new SqlProjectionExpression(new SqlJsonArrayEachItemExpression(source.TypeMapping!), "p0"),
                                new SqlJsonArrayEachExpression(source),
                                null,
                                null,
                                limit,
                                offset));
            }
        }
        else if (
            method.Name == nameof(Enumerable.ToList) ||
            method.Name == nameof(Enumerable.ToArray) ||
            method.Name == nameof(Enumerable.ToHashSet))
        {
            return arguments[0];
        }
        else if (method.Name == nameof(Enumerable.Contains))
        {
            if (arguments.Length != 2)
            {
                return null;
            }

            return new SqlInExpression(_boolType, arguments[1], arguments[0]);
        }
        return null;
    }

    protected virtual SqlExpression CreateCountExpression(SqlExpression source, SqlExpression? predicate)
    {
        if (predicate == null)
        {
            return new SqlJsonArrayLengthExpression(_intType!, source);
        }
        else
        {
            return new SqlSubqueryExpression(_intType!, new SqlSelectExpression(SqlFunctionExpression.CountAll, new SqlJsonArrayEachExpression(source), predicate));
        }
    }

    protected virtual SqlExpression CreateAggregateExpression(
        AggregationType aggregationType,
        SqlExpression source,
        SqlExpression? column)
    {
        var function = aggregationType switch
        {
            AggregationType.Max => SqlFunctionExpression.MaxFunctionName,
            AggregationType.Min => SqlFunctionExpression.MinFunctionName,
            AggregationType.Sum => SqlFunctionExpression.SumFunctionName,
            AggregationType.Average => SqlFunctionExpression.AvgFunctionName,
            _ => null!,
        };

        TypeMapping returnType;
        if (aggregationType == AggregationType.Average)
        {
            _doubleType ??= _mappingSource.FindMapping(typeof(double));
            returnType = _doubleType;
        }
        else
        {
            returnType = GetElementTypeMapping(source.Type);
        }

        var arrayEach = new SqlJsonArrayEachExpression(source);

        if (column == null)
        {
            return new SqlSubqueryExpression(returnType,
                        new SqlSelectExpression(
                            new SqlFunctionExpression(source.TypeMapping!, function, new SqlJsonArrayEachItemExpression(returnType)),
                            arrayEach));
        }
        else
        {
            return new SqlSubqueryExpression(returnType,
                        new SqlSelectExpression(
                            new SqlFunctionExpression(source.TypeMapping!, function, column),
                            arrayEach));
        }
    }

    protected TypeMapping GetElementTypeMapping(Type type)
    {
        var elementType = type.GetEnumerableItemType();
        if (elementType == null)
        {
            ThrowHelper.ThrowTranslateException_CannotGetElementTypeException(type);
        }
        return _mappingSource.FindMapping(elementType);
    }
}