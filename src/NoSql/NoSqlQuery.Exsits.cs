namespace NoSql;

public partial class NoSqlQuery<T>
{
    public bool Any()
    {
        var sql = Dependencies.GeneratorFactory.Create()
            .Generate(Dependencies.ExpressionFactory.CreateExistExpression(TypeInfo, _predicates, false));

        return Dependencies.Connection.ExecuteScalar<bool>(sql);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        var sql = Dependencies.GeneratorFactory.Create()
            .Generate(Dependencies.ExpressionFactory.CreateExistExpression(TypeInfo, _predicates, false));

        return Dependencies.Connection.ExecuteScalarAsync<bool>(sql, cancellationToken);
    }

    public bool All()
    {
        var sql = Dependencies.GeneratorFactory.Create()
            .Generate(Dependencies.ExpressionFactory.CreateExistExpression(TypeInfo, _predicates, true));

        return Dependencies.Connection.ExecuteScalar<bool>(sql);
    }

    public Task<bool> AllAsync(CancellationToken cancellationToken = default)
    {
        var sql = Dependencies.GeneratorFactory.Create()
            .Generate(Dependencies.ExpressionFactory.CreateExistExpression(TypeInfo, _predicates, true));

        return Dependencies.Connection.ExecuteScalarAsync<bool>(sql, cancellationToken);
    }
}
