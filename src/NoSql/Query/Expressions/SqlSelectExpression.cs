namespace NoSql.Query.Expressions;

public class SqlSelectExpression : SqlExpression
{
    public SqlSelectExpression(
        SqlExpression projection,
        SqlTableBaseExpression? table = null,
        SqlExpression? predicate = null,
        SqlOrderingExpression[]? orderings = null,
        int? limit = null,
        int? offset = null)
        : base(projection.Type, projection.TypeMapping)
    {
        Projections = projection;
        Table = table;
        Predicate = predicate;
        Orderings = orderings;
        Limit = limit;
        Offset = offset;
    }

    public SqlExpression Projections { get; }
    public SqlTableBaseExpression? Table { get; }
    public SqlExpression? Predicate { get; }
    public SqlOrderingExpression[]? Orderings { get; }
    public int? Offset { get; set; }
    public int? Limit { get; set; }


    public SqlSelectExpression UpdateProjection(SqlProjectionExpression projection)
    {
        return new SqlSelectExpression(
                projection,
                Table,
                Predicate,
                Orderings,
                Limit,
                Offset);
    }

    public SqlSelectExpression UpdatePredicate(SqlExpression predicate)
    {
        if (Predicate != null)
        {
            predicate = SqlExpressionHelper.ConsolidatePredicate(new SqlExpression[]
            {
                Predicate,
                predicate,
            }, Predicate.TypeMapping!)!;
        }

        return new SqlSelectExpression(
                Projections,
                Table,
                predicate,
                Orderings,
                Limit,
                Offset);
    }

    public SqlSelectExpression UpdateOrdering(SqlOrderingExpression ordering)
    {
        SqlOrderingExpression[] orderings;

        if (Orderings == null)
        {
            orderings = new SqlOrderingExpression[] { ordering };
        }
        else
        {
            orderings = new SqlOrderingExpression[Orderings.Length + 1];
            Array.Copy(Orderings, orderings, Orderings.Length);
            orderings[^1] = ordering;
        }

        return new SqlSelectExpression(
                Projections,
                Table,
                Predicate,
                orderings,
                Limit,
                Offset);
    }

    public SqlSelectExpression UpdateLimitOffset(int? limit, int? offset)
    {
        return new SqlSelectExpression(
                Projections,
                Table,
                Predicate,
                Orderings,
                limit ?? Limit,
                offset ?? Offset);
    }

}
