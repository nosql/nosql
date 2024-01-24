using NoSql.Query;
using NoSql.Query.Expressions;
using NoSql.Storage;

namespace NoSql.SqlServer.Query;

public class SqlServerSqlExpressionFactory : SqlExpressionFactory
{
    public SqlServerSqlExpressionFactory(ISqlTypeMappingSource typeMappingSource) : base(typeMappingSource)
    {
    }

    protected override SqlExpression CreateAggregateExpression(string aggregate, NoSqlTypeInfo table, TypeMapping typeMapping, SqlExpression column, IList<SqlExpression>? predicates, IList<SqlOrderingExpression>? orderings, int? limit = null, int? offset = null)
    {
        if (aggregate == SqlFunctionExpression.AvgFunctionName && typeMapping.ClrType != column.Type)
        {
            column = new SqlCastExpression(typeMapping, column);
        }

        return base.CreateAggregateExpression(aggregate, table, typeMapping, column, predicates, orderings, limit, offset);
    }
}
