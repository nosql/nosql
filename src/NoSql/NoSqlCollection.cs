using NoSql.Scaffolding;

namespace NoSql;

public class NoSqlCollection<T>(NoSqlDependencies dependency, string? name) : NoSqlQuery<T>(dependency, name)
{
    public Task<TableCreateResult> EnsureCreatedAsync(CancellationToken cancellationToken = default)
    {
        return Dependencies.TableCreator.CreateTableAsync(TypeInfo, cancellationToken);
    }

    public TableCreateResult EnsureCreated()
    {
        return Dependencies.TableCreator.CreateTable(TypeInfo);
    }

    public bool Drop()
    {
        return Dependencies.TableCreator.DropTable(TypeInfo.Name);
    }

    public Task<bool> DropAsync(CancellationToken cancellationToken = default)
    {
        return Dependencies.TableCreator.DropTableAsync(TypeInfo.Name, cancellationToken);
    }

    public virtual T? Find(params object[] key)
    {
        string sql = Dependencies.GeneratorFactory.Create().Generate(Dependencies.ExpressionFactory.CreateSelectExpression(TypeInfo, key));
        return Dependencies.Connection.ExecuteOneOrDefaultOne<T>(sql, TypeInfo, null);
    }

    public virtual Task<T?> FindAsync(object key, CancellationToken cancellationToken = default) => FindAsync([key], cancellationToken);
    
    public virtual Task<T?> FindAsync(object[] key, CancellationToken cancellationToken = default)
    {
        string sql = Dependencies.GeneratorFactory.Create().Generate(Dependencies.ExpressionFactory.CreateSelectExpression(TypeInfo, key));
        return Dependencies.Connection.ExecuteOneOrDefaultAsync<T>(sql, TypeInfo, null, cancellationToken);
    }

    public virtual int Insert(T value) => Insert([value]);

    public virtual int Insert(T[] values)
    {
        var sql = Dependencies.GeneratorFactory
            .Create()
            .Generate(Dependencies.ExpressionFactory.CreateInsertExpression(TypeInfo, values));

        return Dependencies.Connection.ExecuteNonQuery(sql);
    }

    public virtual Task<int> InsertAsync(T value,CancellationToken cancellationToken = default) => InsertAsync([value], cancellationToken);

    public virtual Task<int> InsertAsync(T[] values, CancellationToken cancellationToken = default)
    {
        var sql = Dependencies.GeneratorFactory
            .Create()
            .Generate(Dependencies.ExpressionFactory.CreateInsertExpression(TypeInfo, values));

        return Dependencies.Connection.ExecuteNonQueryAsync(sql, cancellationToken);
    }
}
