using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql;

public partial class NoSqlQuery<T>
{
    public int Count()
    {
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateCountExpression(TypeInfo, _predicates, _limit, _offset));
        return Dependencies.Connection.ExecuteScalar<int>(sql);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateCountExpression(TypeInfo, _predicates, _limit, _offset));
        return Dependencies.Connection.ExecuteScalarAsync<int>(sql, cancellationToken);
    }

    public TResult Min<TResult>(Expression<Func<T, TResult>> selector) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateMinExpression(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalar<TResult>(sql);
    }

    public TResult Max<TResult>(Expression<Func<T, TResult>> selector) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateMaxExpression(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalar<TResult>(sql);
    }

    public Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateMinExpression(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalarAsync<TResult>(sql, cancellationToken);
    }

    public Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateMaxExpression(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalarAsync<TResult>(sql, cancellationToken);
    }

    public int Sum(Expression<Func<T, int>> selector) => Sum<int>(selector);
    public long Sum(Expression<Func<T, long>> selector) => Sum<long>(selector);
    public float Sum(Expression<Func<T, float>> selector) => Sum<float>(selector);
    public double Sum(Expression<Func<T, double>> selector) => Sum<double>(selector);
    public decimal Sum(Expression<Func<T, decimal>> selector) => Sum<decimal>(selector);

    public Task<int> SumAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken = default) => SumAsync<int>(selector, cancellationToken);
    public Task<long> SumAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken = default) => SumAsync<long>(selector, cancellationToken);
    public Task<float> SumAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken = default) => SumAsync<float>(selector, cancellationToken);
    public Task<double> SumAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken = default) => SumAsync<double>(selector, cancellationToken);
    public Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default) => SumAsync<decimal>(selector, cancellationToken);

    public double Average(Expression<Func<T, int>> selector) => Average<double, int>(selector);
    public double Average(Expression<Func<T, long>> selector) => Average<double, long>(selector);
    public float Average(Expression<Func<T, float>> selector) => Average<float, float>(selector);
    public double Average(Expression<Func<T, double>> selector) => Average<double, double>(selector);
    public decimal Average(Expression<Func<T, decimal>> selector) => Average<decimal, decimal>(selector);

    public Task<double> AverageAsync(Expression<Func<T, int>> selector, CancellationToken cancellationToken = default) => AverageAsync<double, int>(selector, cancellationToken);
    public Task<double> AverageAsync(Expression<Func<T, long>> selector, CancellationToken cancellationToken = default) => AverageAsync<double, long>(selector, cancellationToken);
    public Task<float> AverageAsync(Expression<Func<T, float>> selector, CancellationToken cancellationToken = default) => AverageAsync<float, float>(selector, cancellationToken);
    public Task<double> AverageAsync(Expression<Func<T, double>> selector, CancellationToken cancellationToken = default) => AverageAsync<double, double>(selector, cancellationToken);
    public Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, CancellationToken cancellationToken = default) => AverageAsync<decimal, decimal>(selector, cancellationToken);

    protected virtual TResult Sum<TResult>(Expression<Func<T, TResult>> selector) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateSumExpression(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalar<TResult>(sql);
    }

    protected virtual Task<TResult> SumAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateSumExpression(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalarAsync<TResult>(sql, cancellationToken);
    }

    protected virtual TResult Average<TResult, TProperty>(Expression<Func<T, TProperty>> selector) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateAverageExpression<TResult>(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalar<TResult>(sql);
    }

    public Task<TResult> AverageAsync<TResult, TProperty>(Expression<Func<T, TProperty>> selector, CancellationToken cancellationToken = default) where TResult : unmanaged
    {
        var column = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);
        var sql = Dependencies.GeneratorFactory
                .Create()
                .Generate(Dependencies.ExpressionFactory.CreateAverageExpression<TResult>(TypeInfo, column, _predicates, _orderings, _limit, _offset));
        return Dependencies.Connection.ExecuteScalarAsync<TResult>(sql, cancellationToken);
    }
}
