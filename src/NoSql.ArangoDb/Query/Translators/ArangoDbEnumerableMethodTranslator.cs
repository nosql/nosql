using NoSql.ArangoDb.Storage;
using NoSql.Query.Expressions;
using NoSql.Query.Translators;
using NoSql.Storage;

namespace NoSql.ArangoDb.Query.Translators;

public class ArangoDbEnumerableMethodTranslator : EnumerableMethodTranslator
{
    public ArangoDbEnumerableMethodTranslator(ISqlTypeMappingSource mappingSource) : base(mappingSource)
    {
    }

    protected override SqlExpression CreateCountExpression(SqlExpression source, SqlExpression? predicate)
    {
        if (predicate == null)
        {
            return new SqlJsonArrayLengthExpression(ArangoDbTypeMappingSource.IntType!, source);
        }
        else
        {
            return new SqlFunctionExpression(ArangoDbTypeMappingSource.IntType, "LENGTH", new SqlSelectExpression(
                new SqlJsonArrayEachItemExpression(source.TypeMapping!),
                new SqlJsonArrayEachExpression(source),
                predicate));
        }
    }

    protected override SqlExpression CreateAggregateExpression(AggregationType aggregationType, SqlExpression source, SqlExpression? column)
    {
        var function = aggregationType switch
        {
            AggregationType.Max => SqlFunctionExpression.MaxFunctionName,
            AggregationType.Min => SqlFunctionExpression.MinFunctionName,
            AggregationType.Sum => SqlFunctionExpression.SumFunctionName,
            AggregationType.Average => SqlFunctionExpression.AvgFunctionName,
            _ => null!,
        };

        var returnType = aggregationType == AggregationType.Average ? ArangoDbTypeMappingSource.DoubleType : GetElementTypeMapping(source.Type);
        var arrayEach = new SqlJsonArrayEachExpression(source);

        SqlSelectExpression subquery;
        if (column == null)
        {
            subquery = new SqlSelectExpression(new SqlProjectionListExpression(source.TypeMapping!, Array.Empty<SqlProjectionExpression>()), arrayEach);
        }
        else
        {
            subquery = new SqlSelectExpression(column, arrayEach);
        }

        return new SqlFunctionExpression(returnType, function, subquery);
    }
}