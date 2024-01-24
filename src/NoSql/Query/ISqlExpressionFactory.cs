using NoSql.Query.Expressions;
using NoSql.Storage;

namespace NoSql.Query;

public interface ISqlExpressionFactory
{
    SqlSelectExpression CreateSelectExpression(NoSqlTypeInfo table, object[] key);

    SqlSelectExpression CreateSelectExpression(NoSqlTypeInfo table,
                                               SqlExpression projections,
                                               IList<SqlExpression>? predicates,
                                               SqlOrderingExpression[]? orderings,
                                               int? limit,
                                               int? offset);

    SqlExpression CreateDeleteExpression(NoSqlTypeInfo table, IList<SqlExpression>? predicates);

    SqlExpression CreateInsertExpression<T>(NoSqlTypeInfo table, T[] items);

    SqlExpression CreateUpdateExpression(NoSqlTypeInfo table,
                                         IDictionary<SqlExpression, SqlExpression> setters,
                                         IList<SqlExpression>? predicates);
    SqlExpression CreateUpdateExpression(NoSqlTypeInfo table,
                                         SqlExpression patch,
                                         IList<SqlExpression>? predicates);

    SqlExpression CreateExistExpression(NoSqlTypeInfo table,
                                        IList<SqlExpression>? predicates,
                                        bool reverseCondition);
    SqlExpression CreateMinExpression(NoSqlTypeInfo table,
                                        SqlExpression column,
                                        IList<SqlExpression>? predicates,
                                        IList<SqlOrderingExpression>? orderings,
                                        int? limit = null,
                                        int? offset = null);

    SqlExpression CreateMaxExpression(NoSqlTypeInfo table,
                                      SqlExpression column,
                                      IList<SqlExpression>? predicates,
                                      IList<SqlOrderingExpression>? orderings,
                                      int? limit = null,
                                      int? offset = null);

    SqlExpression CreateSumExpression(NoSqlTypeInfo table,
                                      SqlExpression column,
                                      IList<SqlExpression>? predicates,
                                      IList<SqlOrderingExpression>? orderings,
                                      int? limit = null,
                                      int? offset = null);

    SqlExpression CreateAverageExpression<TResult>(NoSqlTypeInfo table,
                                                   SqlExpression column,
                                                   IList<SqlExpression>? predicates,
                                                   IList<SqlOrderingExpression>? orderings,
                                                   int? limit = null,
                                                   int? offset = null) 
                                        where TResult : unmanaged;

    SqlExpression CreateCountExpression(NoSqlTypeInfo table,
                                        IList<SqlExpression>? predicates,
                                        int? limit = null,
                                        int? offset = null);
}
