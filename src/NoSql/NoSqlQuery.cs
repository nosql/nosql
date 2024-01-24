using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Linq.Expressions;

namespace NoSql;

public partial class NoSqlQuery<T> 
{
    protected internal readonly NoSqlDependencies Dependencies;
    private readonly List<SqlExpression> _predicates;
    private readonly List<SqlOrderingExpression> _orderings;
    private int? _limit;
    private int? _offset;

    public NoSqlTypeInfo TypeInfo { get; }

    public NoSqlQuery(NoSqlDependencies dependency, string? name)
    {
        Dependencies = dependency;
        TypeInfo = dependency.TableInfoResolver.GetTypeInfo(typeof(T), name);

        _predicates = new();
        _orderings = new();
    }

    public NoSqlQuery(NoSqlQuery<T> query)
    {
        Dependencies = query.Dependencies;
        TypeInfo = query.TypeInfo;
        TypeInfo = query.TypeInfo;
        _predicates = new List<SqlExpression>(query._predicates);
        _orderings = new List<SqlOrderingExpression>(query._orderings);
        _offset = query._offset;
        _limit = query._limit;
    }

    public NoSqlQuery<T> Where(Expression<Func<T, bool>> predicate)
    {
        var exp = Dependencies.TranslatingFactory
            .Create(predicate.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(predicate.Body);
        var query = new NoSqlQuery<T>(this);
        query._predicates.Add(exp);
        return query;
    }

    public NoSqlQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> ordering) => OrderBy(ordering, true);

    public NoSqlQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression, bool descending = false)
    {
        var query = new NoSqlQuery<T>(this);
        var exp = Dependencies.TranslatingFactory
            .Create(expression.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(expression.Body);
        query._orderings.Add(new SqlOrderingExpression(exp, descending));
        return query;
    }

    public NoSqlQuery<T> Limit(int offset, int limit)
    {
        var query = new NoSqlQuery<T>(this)
        {
            _offset = offset,
            _limit = limit
        };
        return query;
    }

    public NoSqlQuery<T> Take(int limit)
    {
        var query = new NoSqlQuery<T>(this)
        {
            _limit = limit
        };
        return query;
    }

    public NoSqlQuery<T> Skip(int offset)
    {
        var query = new NoSqlQuery<T>(this)
        {
            _offset = offset
        };
        return query;
    }

    internal NoSqlQuery<T> OrderBy(SqlExpression ordering, bool descending = false)
    {
        var query = new NoSqlQuery<T>(this);
        query._orderings.Add(new SqlOrderingExpression(ordering, descending));
        return query;
    }

    internal NoSqlQuery<T> Where(SqlExpression predicate)
    {
        var query = new NoSqlQuery<T>(this);
        query._predicates.Add(predicate);
        return query;
    }

}
