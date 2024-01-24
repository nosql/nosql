using NoSql.Storage;
using System.Linq.Expressions;

namespace NoSql.Query.Expressions;

public static class SqlExpressionHelper
{
    public static SqlExpression? ConsolidatePredicate(IList<SqlExpression>? expressions, TypeMapping boolTypeMapping)
    {
        if (expressions == null || expressions.Count == 0)
        {
            return null;
        }

        if (expressions.Count == 1)
        {
            return expressions[0];
        }

        var exp = new SqlBinaryExpression(
            boolTypeMapping,
            ExpressionType.AndAlso,
            expressions[0],
            expressions[1]);

        for (int i = 2; i < expressions.Count; i++)
        {
            exp = new SqlBinaryExpression(
                boolTypeMapping,
                ExpressionType.AndAlso,
                exp,
                expressions[i]);
        }

        return exp;
    }

    public static bool CanConsolidateProjectionOrPredicate(SqlExpression source)
    {
        if (source is not SqlJsonBuildArrayExpression subquery)
            return false;

        if (subquery.Subquery.Projections is SqlJsonArrayEachItemExpression)
            return true;

        if (subquery.Subquery.Projections is SqlProjectionExpression projection &&
            projection.Expression is SqlJsonArrayEachItemExpression)
            return true;

        return false;
    }

    public static List<SqlColumnValueSetExpression> ConsolidateWithJsonSetNest(IList<SqlColumnValueSetExpression> setters)
    {
        List<SqlColumnValueSetExpression> newSetters = new(setters.Count);
        foreach (var setter in setters)
        {
            var value = setter.Value;
            if (value is SqlJsonMergeExpression patch)
            {
                value = ConvertJsonObjectToJsonSetNest(patch.Expression, patch.Value);
                if (value == null)
                {
                    continue;
                }
            }

            var existSetter = newSetters.FirstOrDefault(x => x.Column.Name == setter.Column.Name);
            if (existSetter == null)
            {
                newSetters.Add(new SqlColumnValueSetExpression(setter.Column, value));
            }
            else if (existSetter.Value is SqlJsonSetExpression set1)
            {
                if (value is SqlJsonSetExpression set2)
                {
                    newSetters.Remove(existSetter);
                    newSetters.Add(new SqlColumnValueSetExpression(setter.Column, new SqlJsonSetExpression(set1, set2.Path, set2.Value)));
                }
            }
        }
        return newSetters;
    }

    public static List<SqlColumnValueSetExpression> ConsolidateWithJsonMerge(IList<SqlColumnValueSetExpression> setters)
    {
        List<SqlColumnValueSetExpression> newSetters = new(setters.Count);
        foreach (var setter in setters)
        {
            var value = setter.Value;
            if (setter.Value is SqlJsonSetExpression set &&
                set.Path.Length > 0 &&
                set.Value is not SqlJsonObjectExpression)
            {
                SqlJsonObjectExpression obj = new(set.Expression.TypeMapping!);
                obj.Merge(set.Path.Select(x => x.PropertyName!).ToArray(), set.Value);
                value = new SqlJsonMergeExpression(set.Expression, obj);
            }

            var existSetter = newSetters.FirstOrDefault(x => x.Column.Name == setter.Column.Name);

            if (existSetter == null)
            {
                newSetters.Add(new SqlColumnValueSetExpression(setter.Column, value));
            }
            else
            {
                if (existSetter.Value is SqlJsonMergeExpression patch1)
                {
                    if (value is SqlJsonMergeExpression patch2)
                    {
                        patch1.Value.Merge(patch2.Value);
                    }
                }
            }
        }
        return newSetters;
    }

    private static SqlJsonSetExpression? ConvertJsonObjectToJsonSetNest(SqlExpression column, SqlJsonObjectExpression value)
    {
        SqlJsonSetExpression? set = null;
        ConvertJsonObjectToJsonSetNest(Array.Empty<PathSegment>(), value);

        return set;

        void ConvertJsonObjectToJsonSetNest(PathSegment[] basePath, SqlJsonObjectExpression obj)
        {
            foreach (var property in obj.Properties)
            {
                PathSegment[] path = new PathSegment[basePath.Length + 1];
                Array.Copy(basePath, path, basePath.Length);
                path[^1] = new PathSegment(property.Key);

                if (property.Value is SqlJsonObjectExpression value)
                {
                    ConvertJsonObjectToJsonSetNest(path, value);
                }
                else
                {
                    if (set == null)
                    {
                        set = new SqlJsonSetExpression(column, path, property.Value);
                    }
                    else
                    {
                        set = new SqlJsonSetExpression(set, path, property.Value);
                    }
                }
            }
        }
    }

}
