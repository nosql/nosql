using NoSql.Query;
using NoSql.Query.Expressions;
using NoSql.SqlServer.Storage;
using System.Linq.Expressions;

namespace NoSql.SqlServer.Query;

public class SqlServerSqlGenerator : SqlGenerator
{
    private bool _where;

    protected override void VisitProjection(SqlProjectionExpression expression)
    {
        if (expression.Expression is SqlConstantExpression constantExpression &&
            constantExpression.Type == typeof(double))
        {
            SqlBuilder.Append("CAST(");
            VisitConstant(constantExpression);
            SqlBuilder.Append(" AS float)");
        }
        else
        {
            base.VisitProjection(expression);
        }
    }

    protected override void VisitPredicate(SqlExpression expression)
    {
        _where = true;
        Visit(expression);

        if (IsRequireConvertBoolEqualBinaryExpression(expression))
        {
            SqlBuilder.Append(" = 1");
        }

        _where = false;
    }

    protected override void VisitSelect(SqlSelectExpression expression)
    {
        SqlBuilder.Append("SELECT ");

        bool top = false;
        if (!expression.Offset.HasValue && expression.Limit > 0)
        {
            top = true;
            SqlBuilder.Append($"TOP {expression.Limit.Value} ");
        }

        Visit(expression.Projections);

        if (expression.Table != null)
        {
            SqlBuilder.Append(" FROM ");

            Visit(expression.Table);

            if (expression.Predicate != null)
            {
                SqlBuilder.Append(" WHERE ");
                VisitPredicate(expression.Predicate);
            }

            if (expression.Orderings != null && expression.Orderings.Length > 0)
            {
                SqlBuilder.Append(" ORDER BY ");

                for (int i = 0; i < expression.Orderings.Length; i++)
                {
                    VisitOrdering(expression.Orderings[i]);
                    if (i < expression.Orderings.Length - 1)
                    {
                        SqlBuilder.Append(",");
                    }
                }

            }

            if (top)
            {
                return;
            }

            if (expression.Limit.HasValue || expression.Offset.HasValue)
            {
                if (expression.Orderings != null && expression.Orderings.Length > 0)
                {
                    SqlBuilder.Append(" ");
                }
                else
                {
                    SqlBuilder.Append(" ORDER BY (SELECT 1) ");
                }
                VisitLimitOffset(expression.Offset, expression.Limit);
            }
        }
    }

    protected override void VisitJsonSet(SqlJsonSetExpression node)
    {
        SqlBuilder.Append($"JSON_MODIFY(");

        Visit(node.Expression);

        SqlBuilder.Append(", '");

        GenerateJsonPath(node.Path);

        SqlBuilder.Append("', ");

        if (node.Value is SqlJsonObjectExpression)
        {
            SqlBuilder.Append("JSON_QUERY(CAST('' as nvarchar(1)) + CAST(");
            Visit(node.Value);
            SqlBuilder.Append(" AS nvarchar(4000)) + CAST('' as nvarchar(1)))");
        }
        else
        {
            Visit(node.Value);
        }
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonExtract(SqlJsonExtractExpression node)
    {
        bool cast = false;
        var typeMapping = SqlServerTypeMappingSource.Default.FindMapping(node.Type);
        if (typeMapping.IsJsonType)
        {
            SqlBuilder.Append("JSON_QUERY(");
        }
        else
        {
            if (typeMapping.ClrType != typeof(string))
            {
                cast = true;
                SqlBuilder.Append("CAST(");
            }

            SqlBuilder.Append("JSON_VALUE(");
        }

        Visit(node.Column);

        SqlBuilder.Append(",'");

        GenerateJsonPath(node.Path);

        SqlBuilder.Append("')");

        if (cast)
        {
            SqlBuilder.Append($" AS {typeMapping.StoreType})");
        }
    }

    protected override void VisitLimitOffset(int? offset, int? limit)
    {
        if (offset.HasValue && limit.HasValue)
        {
            SqlBuilder.Append($"OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY");
        }
        else if (offset.HasValue)
        {
            SqlBuilder.Append($"OFFSET {offset.Value} ROWS");
        }
        else if (limit.HasValue)
        {
            SqlBuilder.Append($"FETCH NEXT {limit} ROWS ONLY");
        }
    }

    protected override void VisitJsonObject(SqlJsonObjectExpression expression)
    {
        SqlBuilder.Append($"JSON_OBJECT(");
        int i = 0;
        foreach (var initializer in expression.Properties)
        {
            SqlBuilder.Append($"'{initializer.Key}':");

            if (initializer.Value is SqlColumnExpression column && column.TypeMapping?.IsJsonType == true)
            {
                SqlBuilder.Append($"JSON_QUERY(");

                VisitColumn(column);

                SqlBuilder.Append(")");
            }
            else
            {
                Visit(initializer.Value);
            }

            i++;
            if (i < expression.Properties.Count)
            {
                SqlBuilder.Append(',');
            }
        }

        SqlBuilder.Append(")");
    }

    protected override void VisitJsonArrayEach(SqlJsonArrayEachExpression node)
    {
        SqlBuilder.Append("OPENJSON(");
        Visit(node.Expression);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonArrayEachItem(SqlJsonArrayEachItemExpression node)
    {
        SqlBuilder.Append($"CAST(value AS {node.TypeMapping!.StoreType})");
    }

    protected override void VisitJsonArrayLength(SqlJsonArrayLengthExpression expression)
    {
        SqlBuilder.Append("(SELECT COUNT(*) FROM OPENJSON(");
        Visit(expression.Expression);
        SqlBuilder.Append("))");
    }

    protected override void VisitUnary(SqlUnaryExpression node)
    {
        if (!_where || node.OperatorType != ExpressionType.Not)
        {
            base.VisitUnary(node);
            return;
        }

        bool cast = IsRequireConvertBoolEqualBinaryExpression(node.Operand);
        if (!cast)
        {
            base.VisitUnary(node);
            return;
        }

        SqlBuilder.Append("NOT(");
        Visit(node.Operand);
        SqlBuilder.Append(" = 1)");
    }

    protected override void VisitBinary(SqlBinaryExpression node)
    {
        if (!_where)
        {
            base.VisitBinary(node);
            return;
        }

        bool left = IsRequireConvertBoolEqualBinaryExpression(node.Left);
        bool right = IsRequireConvertBoolEqualBinaryExpression(node.Left);

        var requiresParentheses = left || RequiresParentheses(node, node.Left);

        if (requiresParentheses)
        {
            SqlBuilder.Append("(");
        }

        Visit(node.Left);

        if (requiresParentheses)
        {
            if (left)
            {
                SqlBuilder.Append(" = 1");
            }

            SqlBuilder.Append(")");
        }

        SqlBuilder.Append(GetOperator(node));

        requiresParentheses = right || RequiresParentheses(node, node.Right);
        if (requiresParentheses)
        {
            SqlBuilder.Append("(");
        }

        Visit(node.Right);

        if (requiresParentheses)
        {
            if (right)
            {
                SqlBuilder.Append(" = 1");
            }

            SqlBuilder.Append(")");
        }
    }

    protected override void VisitExists(SqlExistsExpression node)
    {
        if (_where)
        {
            base.VisitExists(node);
        }
        else
        {
            SqlBuilder.Append("(CASE WHEN (");
            base.VisitExists(node);
            SqlBuilder.Append(") THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END)");
        }
    }

    private bool IsRequireConvertBoolEqualBinaryExpression(SqlExpression expression)
    {
        if (expression.Type == typeof(bool))
        {
            if (expression is SqlColumnExpression ||
                expression is SqlJsonExtractExpression ||
                expression is SqlCastExpression)
            {
                return true;
            }
        }
        return false;
    }

}
