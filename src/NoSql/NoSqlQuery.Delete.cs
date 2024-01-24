namespace NoSql;

public partial class NoSqlQuery<T>
{
    public int ExecuteDelete()
    {
        var sql = Dependencies.GeneratorFactory
            .Create()
            .Generate(Dependencies.ExpressionFactory.CreateDeleteExpression(TypeInfo, _predicates));

        return Dependencies.Connection.ExecuteNonQuery(sql);
    }

    public Task<int> ExecuteDeleteAsync(CancellationToken cancellationToken = default)
    {
        var sql = Dependencies.GeneratorFactory
            .Create()
            .Generate(Dependencies.ExpressionFactory.CreateDeleteExpression(TypeInfo, _predicates));

        return Dependencies.Connection.ExecuteNonQueryAsync(sql, cancellationToken);
    }
}
