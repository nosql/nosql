using NoSql.ArangoDb.Storage;
using NoSql.Query;
using NoSql.Query.Expressions;
using NoSql.Storage;

namespace NoSql.ArangoDb.Query;

public class AqlExpressionFactory : SqlExpressionFactory
{
    public AqlExpressionFactory(ISqlTypeMappingSource typeMappingSource) : base(typeMappingSource)
    {
    }

    public override SqlSelectExpression CreateSelectExpression(
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
            returnExpression = new SqlProjectionListExpression(table.TypeMapping, Array.Empty<SqlProjectionExpression>());
        }
        else if(projections is SqlProjectionExpression projection)
        {
            returnExpression = projection.Expression;
        }
        else if (projections is  SqlProjectionListExpression projectionList)
        {
            returnExpression = new SqlJsonObjectExpression(
                projectionList.TypeMapping!,
                projectionList.ProjectionList.ToDictionary(x => x.Alias!, x => x.Expression));
        }
        else
        {
            returnExpression = projections;
        }

        return new SqlSelectExpression(
            returnExpression,
            new SqlTableExpression(table),
            ConsolidatePredicates(predicates),
            orderings,
            limit,
            offset);
    }

    public override SqlExpression CreateCountExpression(NoSqlTypeInfo table, IList<SqlExpression>? predicates, int? limit = null, int? offset = null)
    {
        var subquery = new SqlSelectExpression(
            new SqlProjectionListExpression(table.TypeMapping,Array.Empty<SqlProjectionExpression>()),
            new SqlTableExpression(table), 
            SqlExpressionHelper.ConsolidatePredicate(predicates, ArangoDbTypeMappingSource.BoolType),
            null, limit, offset);
        return new SqlSelectExpression(new SqlFunctionExpression(ArangoDbTypeMappingSource.IntType, "COUNT", subquery));
    }

    protected override SqlExpression CreateAggregateExpression(string aggregate, NoSqlTypeInfo table, TypeMapping typeMapping, SqlExpression column, IList<SqlExpression>? predicates, IList<SqlOrderingExpression>? orderings, int? limit = null, int? offset = null)
    {
        var subquery = new SqlSelectExpression(
            column,
            new SqlTableExpression(table),
            SqlExpressionHelper.ConsolidatePredicate(predicates, ArangoDbTypeMappingSource.BoolType),
            orderings?.ToArray(),
            limit, offset);
        return new SqlSelectExpression(new SqlFunctionExpression(ArangoDbTypeMappingSource.BoolType, aggregate, subquery));
    }

    protected override bool CanConvertJsonMerge => true;
}