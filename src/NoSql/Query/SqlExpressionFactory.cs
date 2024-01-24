using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NoSql.Query;

public class SqlExpressionFactory : ISqlExpressionFactory
{
    protected readonly ISqlTypeMappingSource TypeMappingSource;
    private TypeMapping? _boolTypeMapping;
    private TypeMapping? _intTypeMapping;

    public SqlExpressionFactory(ISqlTypeMappingSource typeMappingSource)
    {
        TypeMappingSource = typeMappingSource;
    }

    public virtual SqlExpression CreateExistExpression(NoSqlTypeInfo table, IList<SqlExpression>? predicates, bool reverseCondition)
    {
        _boolTypeMapping ??= TypeMappingSource.FindMapping(typeof(bool));
        _intTypeMapping ??= TypeMappingSource.FindMapping(typeof(int));

        var condition = ConsolidatePredicates(predicates);
        if (condition != null && reverseCondition)
        {
            condition = new SqlUnaryExpression(_boolTypeMapping, ExpressionType.Not, condition);
        }

        return new SqlSelectExpression(
            new SqlExistsExpression(
                _boolTypeMapping,
                new SqlSelectExpression(
                    new SqlConstantExpression(_intTypeMapping, 1),
                    new SqlTableExpression(table),
                    condition),
                reverseCondition));
    }

    public virtual SqlSelectExpression CreateSelectExpression(
        NoSqlTypeInfo table,
        SqlExpression projections,
        IList<SqlExpression>? predicates,
        SqlOrderingExpression[]? orderings,
        int? limit,
        int? offset)
    {
        SqlExpression returnExpression;
        if (projections is SqlTableExpression tableExpression)
        {
            returnExpression = tableExpression.TableInfo.Projections;
        }
        else if (projections is SqlJsonObjectExpression objectExpression)
        {
            returnExpression = new SqlProjectionListExpression(
                objectExpression.TypeMapping!,
                objectExpression.Properties.Select(x => new SqlProjectionExpression(x.Value, x.Key)).ToArray());
        }
        else if (projections is SqlProjectionExpression or SqlProjectionListExpression)
        {
            returnExpression = projections;
        }
        else
        {
            returnExpression = new SqlProjectionExpression(projections);
        }

        return new SqlSelectExpression(
            returnExpression,
            new SqlTableExpression(table),
            ConsolidatePredicates(predicates),
            orderings,
            limit,
            offset);
    }

    public virtual SqlSelectExpression CreateSelectExpression(NoSqlTypeInfo table, object[] key)
    {
        var primaryKey = table.PrimaryKey;
        if (primaryKey == null || primaryKey.Columns.Length != key.Length)
        {
            ThrowHelper.ThrowTranslateException_PrimaryKeyNotMatched(table);
        }

        _boolTypeMapping ??= TypeMappingSource.FindMapping(typeof(bool));

        List<SqlExpression> predicates = new(primaryKey.Columns.Length);
        for (int i = 0; i < primaryKey.Columns.Length; i++)
        {
            var value = key[i];
            var column = primaryKey.Columns[i];
            if (column.TypeMapping.ClrType != value.GetType())
            {
                ThrowHelper.ThrowTranslateException_ColumnTypeNotExpected(column.Name);
            }

            predicates.Add(new SqlBinaryExpression(_boolTypeMapping, ExpressionType.Equal,
                new SqlColumnExpression(column),
                new SqlConstantExpression(column.TypeMapping, value)));
        }

        return new SqlSelectExpression(
            table.Projections,
            new SqlTableExpression(table),
            ConsolidatePredicates(predicates),
            null,
            1);
    }

    public virtual SqlExpression CreateDeleteExpression(NoSqlTypeInfo table, IList<SqlExpression>? predicates)
    {
        return new SqlDeleteExpression(new SqlTableExpression(table), ConsolidatePredicates(predicates));
    }

    public virtual SqlExpression CreateUpdateExpression(
        NoSqlTypeInfo table,
        IDictionary<SqlExpression, SqlExpression> keyValuePairs,
        IList<SqlExpression>? predicates)
    {
        List<SqlColumnValueSetExpression> setters = new();
        foreach (var setter in keyValuePairs)
        {
            var key = setter.Key;
            var value = setter.Value;

            if (key is SqlCastExpression cast)
            {
                key = cast.Expression;
                value = new SqlCastExpression(value.TypeMapping!, value);
            }

            if (key is SqlJsonExtractExpression jsonExtract)
            {
                key = jsonExtract.Column;
                value = new SqlJsonSetExpression(jsonExtract.Column, jsonExtract.Path, value);
            }

            if (key is not SqlColumnExpression column)
            {
                ThrowHelper.ThrowTranslateException_ExpressionNotExpected(nameof(SqlColumnExpression));
                return null;
            }

            setters.Add(new SqlColumnValueSetExpression(column, value));
        }

        if (CanConvertJsonMerge)
        {
            setters = SqlExpressionHelper.ConsolidateWithJsonMerge(setters);
        }
        else
        {
            setters = SqlExpressionHelper.ConsolidateWithJsonSetNest(setters);
        }

        return new SqlUpdateExpression(setters, new SqlTableExpression(table), ConsolidatePredicates(predicates));
    }

    public virtual SqlExpression CreateUpdateExpression(NoSqlTypeInfo table, SqlExpression patch, IList<SqlExpression>? predicates)
    {
        if (patch is not SqlJsonObjectExpression obj)
        {
            ThrowHelper.ThrowTranslateException_ExpressionNotExpected(nameof(SqlJsonObjectExpression));
            return null;
        }

        var setters = obj.Properties
            .Select(x =>
            {
                if (x.Value is SqlJsonObjectExpression obj)
                {
                    var column = new SqlColumnExpression(table.GetMappedColumn(x.Key));
                    return new SqlColumnValueSetExpression(column, new SqlJsonMergeExpression(column, obj));
                }
                else
                {
                    return new SqlColumnValueSetExpression(new SqlColumnExpression(table.GetMappedColumn(x.Key)), x.Value);
                }
            })
            .ToList();

        if (!CanConvertJsonMerge)
        {
            setters = SqlExpressionHelper.ConsolidateWithJsonSetNest(setters);
        }

        return new SqlUpdateExpression(setters, new SqlTableExpression(table), ConsolidatePredicates(predicates));
    }

    public virtual SqlExpression CreateInsertExpression<T>(NoSqlTypeInfo table, T[] items)
    {
        int length = table.MappedFields.Where(x => !x.AutoIncrement).Count();

        SqlColumnExpression[] columns = new SqlColumnExpression[length];
        SqlExpression[][] values = new SqlExpression[items.Length][];

        for (int i = 0, j = 0; i < table.MappedFields.Length; i++)
        {
            var column = table.MappedFields[i];

            if (column.AutoIncrement)
                continue;

            columns[j] = new SqlColumnExpression(column);
            j++;
        }

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            values[i] = new SqlExpression[length];
            for (int j = 0, k = 0; j < table.MappedFields.Length; j++)
            {
                var column = table.MappedFields[j];
                if (column.AutoIncrement || column.Get == null)
                    continue;

                object? value = column.Get(item!);
                values[i][k] = new SqlConstantExpression(column.TypeMapping, value);
                k++;
            }
        }

        return new SqlInsertExpression(new SqlTableExpression(table), columns, values);
    }

    public virtual SqlExpression CreateMinExpression(NoSqlTypeInfo table, SqlExpression column, IList<SqlExpression>? predicates, IList<SqlOrderingExpression>? orderings, int? limit = null, int? offset = null)
    {
        return CreateAggregateExpression(SqlFunctionExpression.MinFunctionName, table, column.TypeMapping!, column, predicates, orderings, limit, offset);
    }

    public virtual SqlExpression CreateMaxExpression(NoSqlTypeInfo table, SqlExpression column, IList<SqlExpression>? predicates, IList<SqlOrderingExpression>? orderings, int? limit = null, int? offset = null)
    {
        return CreateAggregateExpression(SqlFunctionExpression.MaxFunctionName, table, column.TypeMapping!, column, predicates, orderings, limit, offset);
    }

    public virtual SqlExpression CreateSumExpression(NoSqlTypeInfo table, SqlExpression column, IList<SqlExpression>? predicates, IList<SqlOrderingExpression>? orderings, int? limit = null, int? offset = null)
    {
        return CreateAggregateExpression(SqlFunctionExpression.SumFunctionName, table, column.TypeMapping!, column, predicates, orderings, limit, offset);
    }

    public virtual SqlExpression CreateAverageExpression<TResult>(NoSqlTypeInfo table, SqlExpression column, IList<SqlExpression>? predicates, IList<SqlOrderingExpression>? orderings, int? limit = null, int? offset = null)
        where TResult : unmanaged
    {
        var typeMapping = typeof(TResult) == column.Type ? column.TypeMapping! : TypeMappingSource.FindMapping(typeof(TResult));
        return CreateAggregateExpression(SqlFunctionExpression.AvgFunctionName, table, typeMapping, column, predicates, orderings, limit, offset);
    }

    public virtual SqlExpression CreateCountExpression(NoSqlTypeInfo table, IList<SqlExpression>? predicates, int? limit = null, int? offset = null)
    {
        _intTypeMapping ??= TypeMappingSource.FindMapping(typeof(int));
        return CreateAggregateExpression(SqlFunctionExpression.CountFunctionName, table, _intTypeMapping, new SqlFragmentExpression("*"), predicates, null, limit, offset);
    }

    protected virtual SqlExpression CreateAggregateExpression(string aggregate,
                                                    NoSqlTypeInfo table,
                                                    TypeMapping typeMapping,
                                                    SqlExpression column,
                                                    IList<SqlExpression>? predicates,
                                                    IList<SqlOrderingExpression>? orderings,
                                                    int? limit = null,
                                                    int? offset = null)
    {
        return new SqlSelectExpression(
                new SqlFunctionExpression(typeMapping, aggregate, column),
                new SqlTableExpression(table),
                ConsolidatePredicates(predicates),
                orderings?.ToArray(),
                limit,
                offset);
    }

    protected SqlExpression? ConsolidatePredicates(IList<SqlExpression>? expressions)
    {
        _boolTypeMapping ??= TypeMappingSource.FindMapping(typeof(bool));
        return SqlExpressionHelper.ConsolidatePredicate(expressions, _boolTypeMapping);
    }

    protected virtual bool CanConvertJsonMerge { get; }

}