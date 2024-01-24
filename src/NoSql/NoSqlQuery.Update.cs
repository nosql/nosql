using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql;

public partial class NoSqlQuery<T>
{
    public int ExecuteUpdate(Expression<Func<T, T>> setter)
    {
        var expression = Dependencies.TranslatingFactory
            .Create(setter.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(setter.Body);

        var sql = Dependencies.GeneratorFactory
            .Create()
            .Generate(Dependencies.ExpressionFactory.CreateUpdateExpression(TypeInfo, expression, _predicates));

        return Dependencies.Connection.ExecuteNonQuery(sql);
    }

    public Task<int> ExecuteUpdateAsync(Expression<Func<T, T>> setter, CancellationToken cancellationToken = default)
    {
        var expression = Dependencies.TranslatingFactory
            .Create(setter.Parameters[0].Name!, new SqlTableExpression(TypeInfo))
            .Visit(setter.Body);

        var sql = Dependencies.GeneratorFactory
            .Create()
            .Generate(Dependencies.ExpressionFactory.CreateUpdateExpression(TypeInfo, expression, _predicates));

        return Dependencies.Connection.ExecuteNonQueryAsync(sql, cancellationToken);
    }

    public int ExecuteUpdate(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
    {
        var sql = ToUpdateQueryString(setPropertyCalls);
        return Dependencies.Connection.ExecuteNonQuery(sql);
    }

    public Task<int> ExecuteUpdateAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls, CancellationToken cancellationToken = default)
    {
        var sql = ToUpdateQueryString(setPropertyCalls);
        return Dependencies.Connection.ExecuteNonQueryAsync(sql, cancellationToken);
    }

    private string ToUpdateQueryString(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
    {
        var call = (MethodCallExpression)setPropertyCalls.Body;
        var dic = new Dictionary<Expression, Expression>();
        var parameters = new Dictionary<string, SqlExpression>();

        var tableExpression = new SqlTableExpression(TypeInfo);
        do
        {
            LambdaExpression lambdaProperty = (LambdaExpression)call.Arguments[0];

            var parameterName = lambdaProperty.Parameters[0].Name!;

            if (!parameters.ContainsKey(parameterName))
            {
                parameters.Add(parameterName, tableExpression);
            }

            var property = lambdaProperty.Body;

            var valueExpression = call.Arguments[1];
            if (valueExpression is LambdaExpression lambdaValue)
            {
                parameterName = lambdaValue.Parameters[0].Name!;
                if (!parameters.ContainsKey(parameterName))
                {
                    parameters.Add(parameterName, tableExpression);
                }

                valueExpression = lambdaValue.Body;
            }

            dic.Add(property, valueExpression);
            call = call.Object as MethodCallExpression;

        } while (call != null);

        var setters = new Dictionary<SqlExpression, SqlExpression>(dic.Count);
        var visitor = Dependencies.TranslatingFactory.Create(parameters);
        foreach (var kvp in dic)
        {
            setters.Add(visitor.Visit(kvp.Key), visitor.Visit(kvp.Value));
        }

        return Dependencies.GeneratorFactory.Create().Generate(Dependencies.ExpressionFactory.CreateUpdateExpression(TypeInfo, setters, _predicates));

    }
}
