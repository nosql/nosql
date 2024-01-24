using System.Linq.Expressions;

namespace NoSql;

public static partial class NoSqlQueryExntensions
{
    public static int Count<T>(this NoSqlQuery<T> query, Expression<Func<T, bool>> predicate) => query.Where(predicate).Count();
    public static bool Any<T>(this NoSqlQuery<T> query, Expression<Func<T, bool>> predicate) => query.Where(predicate).Any();
    public static bool All<T>(this NoSqlQuery<T> query, Expression<Func<T, bool>> predicate) => query.Where(predicate).All();
    public static Task<int> CountAsync<T>(this NoSqlQuery<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => query.Where(predicate).CountAsync(cancellationToken);
    public static Task<bool> AnyAsync<T>(this NoSqlQuery<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => query.Where(predicate).AnyAsync(cancellationToken);
    public static Task<bool> AllAsync<T>(this NoSqlQuery<T> query, Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => query.Where(predicate).AllAsync(cancellationToken);
}