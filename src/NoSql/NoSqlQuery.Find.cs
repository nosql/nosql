using NoSql.Extensions;
using NoSql.Query.Expressions;
using NoSql.Storage;
using System.Diagnostics;
using System.Linq.Expressions;

namespace NoSql;

public partial class NoSqlQuery<T>
{
    public T? FindOne()
    {
        var sql = ToQueryString(TypeInfo, TypeInfo.Projections, 1, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteOneOrDefaultOne<T>(sql, TypeInfo, columns);
    }

    public Task<T?> FindOneAsync(CancellationToken cancellationToken = default)
    {
        var sql = ToQueryString(TypeInfo, TypeInfo.Projections, 1, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteOneOrDefaultAsync<T>(sql, TypeInfo, columns, cancellationToken);
    }

    public List<T?> FindAll()
    {
        var sql = ToQueryString(TypeInfo, TypeInfo.Projections, _limit, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection
            .ExecuteEnumerable<T>(sql, TypeInfo, columns)
            .ToList();
    }

    public Task<List<T?>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = ToQueryString(TypeInfo, TypeInfo.Projections, _limit, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection
            .ExecuteEnumerableAsync<T>(sql, TypeInfo, columns, cancellationToken)
            .ToListAsync(cancellationToken);
    }

    public TResult? FindOne<TResult>(Expression<Func<T, TResult>> selector)
    {
        var sql = ToQueryString(selector, out NoSqlTypeInfo returnType, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteOneOrDefaultOne<TResult>(sql, returnType, columns);
    }

    public Task<TResult?> FindOneAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        var sql = ToQueryString(selector, out NoSqlTypeInfo returnType, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteOneOrDefaultAsync<TResult>(sql, returnType, columns, cancellationToken);
    }

    public Task<List<TResult?>> FindAllAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        var sql = ToQueryString(selector, out NoSqlTypeInfo returnType, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection
            .ExecuteEnumerableAsync<TResult>(sql, returnType, columns, cancellationToken)
            .ToListAsync(cancellationToken);
    }

    public List<TResult?> FindAll<TResult>(Expression<Func<T, TResult>> selector)
    {
        var sql = ToQueryString(selector, out NoSqlTypeInfo returnType, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteEnumerable<TResult>(sql, returnType, columns).ToList();
    }

    public IEnumerable<TResult?> ToEnumerable<TResult>(Expression<Func<T, TResult>> selector)
    {
        var sql = ToQueryString(selector, out NoSqlTypeInfo returnType, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteEnumerable<TResult>(sql, returnType, columns);
    }

    public IAsyncEnumerable<TResult?> ToAsyncEnumerable<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
    {
        var sql = ToQueryString(selector, out NoSqlTypeInfo returnType, out NoSqlFieldInfo[]? columns);
        return Dependencies.Connection.ExecuteEnumerableAsync<TResult>(sql, returnType, columns, cancellationToken);
    }

    public string ToQueryString() => ToQueryString(TypeInfo, TypeInfo.Projections, _limit, out _);

    private string ToQueryString<TResult>(
        Expression<Func<T, TResult>> selector,
        out NoSqlTypeInfo returnType,
        out NoSqlFieldInfo[]? columns)
    {
        var projections = Dependencies.TranslatingFactory
            .Create(selector.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(selector.Body);

        returnType = Dependencies.TableInfoResolver.GetTypeInfo(selector.Body.Type);
        return ToQueryString(returnType, projections, _limit, out columns);
    }

    private string ToQueryString(
        NoSqlTypeInfo returnType,
        SqlExpression projections,
        int? limit,
        out NoSqlFieldInfo[]? columns)
    {
        var expression = Dependencies.ExpressionFactory.CreateSelectExpression(
                TypeInfo,
                projections,
                _predicates,
                _orderings.ToArray(),
                limit,
                _offset);

        if (expression.Projections is SqlProjectionListExpression projectionsList)
        {
            columns = new NoSqlFieldInfo[projectionsList.ProjectionList.Length];
            for (int i = 0; i < projectionsList.ProjectionList.Length; i++)
            {
                var projection = projectionsList.ProjectionList[i];
                for (int j = 0; j < returnType.Fields.Length; j++)
                {
                    if (returnType.Fields[j].Name == projection.Alias)
                    {
                        columns[i] = returnType.Fields[j];
                        break;
                    }
                }
                Debug.Assert(columns[i] != null);
            }
        }
        else
        {
            columns = null;
        }

        return Dependencies.GeneratorFactory.Create().Generate(expression);
    }
}
