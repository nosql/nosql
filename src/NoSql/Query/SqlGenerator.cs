using NoSql.Query.Expressions;
using System.Collections;
using System.Linq.Expressions;
using System.Text;

namespace NoSql.Query;

public class SqlGenerator : SqlExpressionVisitor, ISqlGenerator
{
    private static readonly Dictionary<ExpressionType, string> OperatorMap = new()
    {
        { ExpressionType.Equal, " = " },
        { ExpressionType.NotEqual, " <> " },
        { ExpressionType.GreaterThan, " > " },
        { ExpressionType.GreaterThanOrEqual, " >= " },
        { ExpressionType.LessThan, " < " },
        { ExpressionType.LessThanOrEqual, " <= " },
        { ExpressionType.AndAlso, " AND " },
        { ExpressionType.OrElse, " OR " },
        { ExpressionType.Add, " + " },
        { ExpressionType.Subtract, " - " },
        { ExpressionType.Multiply, " * " },
        { ExpressionType.Divide, " / " },
        { ExpressionType.Modulo, " % " },
        { ExpressionType.And, " & " },
        { ExpressionType.Or, " | " }
    };

    protected readonly StringBuilder SqlBuilder = new();

    public string Generate(SqlExpression expression)
    {
        Visit(expression);
        return SqlBuilder.ToString();
    }

    protected override void VisitFragment(SqlFragmentExpression expression)
    {
        SqlBuilder.Append(expression.Sql);
    }

    protected override void VisitFunction(SqlFunctionExpression expression)
    {
        SqlBuilder.Append($"{expression.Name}(");
        for (int i = 0; i < expression.Arguments.Length; i++)
        {
            SqlExpression arg = expression.Arguments[i];
            Visit(arg);

            if (i < expression.Arguments.Length - 1)
            {
                SqlBuilder.Append(", ");
            }
        }
        SqlBuilder.Append(")");
    }

    protected override void VisitInsert(SqlInsertExpression expression)
    {
        SqlBuilder.Append("INSERT INTO ");
        VisitTable(expression.Table);
        SqlBuilder.Append(" (");

        for (int i = 0; i < expression.Columns.Length; i++)
        {
            SqlColumnExpression? column = expression.Columns[i];
            SqlBuilder.Append($"\"{column.Name}\"");

            if (i < expression.Columns.Length - 1)
            {
                SqlBuilder.Append(',');
            }
        }

        SqlBuilder.Append(") VALUES");

        for (int i = 0; i < expression.Values.Length; i++)
        {
            SqlBuilder.Append("(");

            SqlExpression[]? values = expression.Values[i];

            for (int j = 0; j < values.Length; j++)
            {
                Visit(values[j]);
                if (j < values.Length - 1)
                {
                    SqlBuilder.Append(',');
                }
            }

            SqlBuilder.Append(")");

            if (i < expression.Values.Length - 1)
            {
                SqlBuilder.Append(',');
            }
        }
    }

    protected virtual void VisitLimitOffset(int? offset, int? limit)
    {
        if (offset.HasValue && limit.HasValue)
        {
            SqlBuilder.Append($"LIMIT {limit} OFFSET {offset}");
        }
        else if (offset.HasValue)
        {
            SqlBuilder.Append($"OFFSET {offset}");
        }
        else if(limit.HasValue)
        {
            SqlBuilder.Append($"LIMIT {limit}");
        }
    }

    protected override void VisitOrdering(SqlOrderingExpression expression)
    {
        Visit(expression.Expression);

        if (expression.IsDescending)
        {
            SqlBuilder.Append(" DESC");
        }
    }

    protected override void VisitSelect(SqlSelectExpression expression)
    {
        SqlBuilder.Append("SELECT ");

        if (expression.Projections == null)
        {
            SqlBuilder.Append("1");
        }
        else
        {
            Visit(expression.Projections);
        }

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

            if (expression.Limit.HasValue || expression.Offset.HasValue)
            {
                SqlBuilder.Append(" ");
                VisitLimitOffset(expression.Offset, expression.Limit);
            }
        }
    }

    protected override void VisitDelete(SqlDeleteExpression expression)
    {
        SqlBuilder.Append("DELETE FROM ");
        VisitTable(expression.Table);

        if (expression.Predicate != null)
        {
            SqlBuilder.Append(" WHERE ");
            VisitPredicate(expression.Predicate);
        }
    }

    protected override void VisitUpdate(SqlUpdateExpression expression)
    {
        SqlBuilder.Append("UPDATE ");

        VisitTable(expression.Table);

        SqlBuilder.Append(" SET ");

        int i = 0;

        foreach (var setter in expression.Setters)
        {
            VisitColumnValueSet(setter);

            i++;

            if (i < expression.Setters.Count)
            {
                SqlBuilder.Append(", ");
            }
        }

        if (expression.Predicate != null)
        {
            SqlBuilder.Append(" WHERE ");

            VisitPredicate(expression.Predicate);
        }
    }

    protected virtual void VisitPredicate(SqlExpression expression) => Visit(expression);

    protected override void VisitColumnValueSet(SqlColumnValueSetExpression expression)
    {
        SqlBuilder.Append(DelimitIdentifier(expression.Column.Name));
        SqlBuilder.Append(" = ");
        Visit(expression.Value);
    }

    protected override void VisitTable(SqlTableExpression expression)
    {
        SqlBuilder.Append(DelimitIdentifier(expression.Name));
    }

    protected override void VisitColumn(SqlColumnExpression expression)
    {
        SqlBuilder.Append(DelimitIdentifier(expression.Name));
    }

    protected override void VisitProjection(SqlProjectionExpression expression)
    {
        Visit(expression.Expression);

        if (expression.Alias != null)
        {
            SqlBuilder.Append($" AS '{expression.Alias}'");
        }
    }

    protected override void VisitProjectionList(SqlProjectionListExpression expression)
    {
        if (expression.ProjectionList.Length == 0)
        {
            SqlBuilder.Append("1");
        }
        else
        {
            for (int i = 0; i < expression.ProjectionList.Length; i++)
            {
                var projection = expression.ProjectionList[i];
                VisitProjection(projection);

                if (i < expression.ProjectionList.Length - 1)
                {
                    SqlBuilder.Append(",");
                }
            }
        }
    }

    protected override void VisitUnary(SqlUnaryExpression expression)
    {
        switch (expression.OperatorType)
        {
            case ExpressionType.Convert:
                {
                    if (expression.Type == expression.Operand.Type)
                    {
                        Visit(expression.Operand);
                    }
                    else
                    {
                        if (expression.TypeMapping == null)
                        {
                            Visit(expression.Operand);
                        }
                        else
                        {
                            SqlBuilder.Append("CAST(");
                            Visit(expression.Operand);
                            SqlBuilder.Append($" AS {expression.TypeMapping.StoreType})");
                        }
                    }
                    break;
                }
            case ExpressionType.Not
                when expression.Type == typeof(bool):
                {
                    SqlBuilder.Append("NOT(");
                    Visit(expression.Operand);
                    SqlBuilder.Append(")");
                    break;
                }

            case ExpressionType.Not:
                {
                    SqlBuilder.Append("~");

                    var requiresBrackets = RequiresParentheses(expression, expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append("(");
                    }

                    Visit(expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append(")");
                    }

                    break;
                }

            case ExpressionType.Equal:
                {
                    var requiresBrackets = RequiresParentheses(expression, expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append("(");
                    }

                    Visit(expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append(")");
                    }

                    SqlBuilder.Append(" IS NULL");
                    break;
                }

            case ExpressionType.NotEqual:
                {
                    var requiresBrackets = RequiresParentheses(expression, expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append("(");
                    }

                    Visit(expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append(")");
                    }

                    SqlBuilder.Append(" IS NOT NULL");
                    break;
                }

            case ExpressionType.Negate:
                {
                    SqlBuilder.Append("-");
                    var requiresBrackets = RequiresParentheses(expression, expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append("(");
                    }

                    Visit(expression.Operand);
                    if (requiresBrackets)
                    {
                        SqlBuilder.Append(")");
                    }

                    break;
                }
        }
    }

    protected override void VisitBinary(SqlBinaryExpression expression)
    {
        var requiresParentheses = RequiresParentheses(expression, expression.Left);

        if (requiresParentheses)
        {
            SqlBuilder.Append("(");
        }

        Visit(expression.Left);

        if (requiresParentheses)
        {
            SqlBuilder.Append(")");
        }

        SqlBuilder.Append(GetOperator(expression));

        requiresParentheses = RequiresParentheses(expression, expression.Right);
        if (requiresParentheses)
        {
            SqlBuilder.Append("(");
        }

        Visit(expression.Right);

        if (requiresParentheses)
        {
            SqlBuilder.Append(")");
        }
    }

    protected override void VisitLike(SqlLikeExpression expression)
    {
        Visit(expression.Expression);
        SqlBuilder.Append($" LIKE '{expression.Value}'");
    }

    protected override void VisitSubquery(SqlSubqueryExpression expression)
    {
        SqlBuilder.Append("(");
        Visit(expression.Subquery);
        SqlBuilder.Append(")");
    }

    protected override void VisitCast(SqlCastExpression expression)
    {
        SqlBuilder.Append("CAST(");
        Visit(expression.Expression);
        SqlBuilder.Append($" AS {expression.TypeMapping!.StoreType})");
    }

    protected override void VisitExists(SqlExistsExpression expression)
    {
        if (expression.IsNegated)
        {
            SqlBuilder.Append("NOT EXISTS(");
        }
        else
        {
            SqlBuilder.Append("EXISTS(");
        }
        Visit(expression.Expression);
        SqlBuilder.Append(")");
    }

    protected override void VisitIn(SqlInExpression expression)
    {
        Visit(expression.Item);
        SqlBuilder.Append(expression.IsNegated ? " NOT IN (" : " IN (");
        if (expression.Values is SqlConstantExpression constantExpression)
        {
            List<SqlConstantExpression> values = new();
            var enumerator = ((IEnumerable)constantExpression.Value!).GetEnumerator();
            while (enumerator.MoveNext())
            {
                values.Add(new SqlConstantExpression(expression.Item.TypeMapping!, enumerator.Current));
            }

            for (int i = 0; i < values.Count; i++)
            {
                SqlConstantExpression? item = values[i];
                Visit(item);

                if (i < values.Count - 1)
                    SqlBuilder.Append(", ");
            }
        }
        else
        {
            Visit(expression.Values);
        }
        SqlBuilder.Append(")");
    }

    protected override void VisitConstant(SqlConstantExpression expression)
    {
        SqlBuilder.Append(expression.TypeMapping!.GenerateSqlLiteral(expression.Value));
    }

    protected override void VisitJsonArrayEachItem(SqlJsonArrayEachItemExpression expression) => SqlBuilder.Append("value");

    protected override void VisitJsonMerge(SqlJsonMergeExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);
   
    protected override void VisitJsonBuildArray(SqlJsonBuildArrayExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);

    protected override void VisitJsonArrayLength(SqlJsonArrayLengthExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);

    protected override void VisitJsonSet(SqlJsonSetExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);

    protected override void VisitJsonExtract(SqlJsonExtractExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);

    protected override void VisitJsonObject(SqlJsonObjectExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);

    protected override void VisitJsonArrayEach(SqlJsonArrayEachExpression expression) => ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);

    protected virtual void GenerateJsonPath(PathSegment[] path)
    {
        SqlBuilder.Append("$");

        foreach (var segment in path)
        {
            if (segment.PropertyName == null)
            {
                SqlBuilder.Append('[');
                Visit(segment.ArrayIndex!);
                SqlBuilder.Append(']');
            }
            else
            {
                SqlBuilder.Append('.');
                SqlBuilder.Append(segment.PropertyName);
            }
        }
    }

    protected virtual string DelimitIdentifier(string identifier) => "\"" + identifier + "\"";


    protected virtual string GetOperator(SqlBinaryExpression expression) => OperatorMap[expression.OperatorType];

    protected virtual bool RequiresParentheses(SqlExpression outerExpression, SqlExpression innerExpression)
    {
        int outerPrecedence, innerPrecedence;

        // Convert is rendered as a function (CAST()) and not as an operator, so we never need to add parentheses around the inner
        if (outerExpression is SqlUnaryExpression { OperatorType: ExpressionType.Convert })
        {
            return false;
        }

        switch (innerExpression)
        {
            case SqlUnaryExpression innerUnaryExpression:
                {
                    // If the same unary operator is used in both outer and inner (e.g. NOT NOT), no parentheses are needed
                    if (outerExpression is SqlUnaryExpression outerUnary
                        && innerUnaryExpression.OperatorType == outerUnary.OperatorType)
                    {
                        // ... except for double negative (--), which is interpreted as a comment in SQL
                        return innerUnaryExpression.OperatorType == ExpressionType.Negate;
                    }

                    // If the provider defined precedence for the two expression, use that
                    if (TryGetOperatorInfo(outerExpression, out outerPrecedence, out _)
                        && TryGetOperatorInfo(innerExpression, out innerPrecedence, out _))
                    {
                        return outerPrecedence >= innerPrecedence;
                    }

                    // Otherwise, wrap IS (NOT) NULL operation, except if it's in a logical operator
                    if (innerUnaryExpression.OperatorType is ExpressionType.Equal or ExpressionType.NotEqual
                        && outerExpression is not SqlBinaryExpression
                        {
                            OperatorType: ExpressionType.AndAlso or ExpressionType.OrElse or ExpressionType.Not
                        })
                    {
                        return true;
                    }

                    return false;
                }

            case SqlBinaryExpression innerBinaryExpression:
                {
                    // Precedence-wise AND is above OR but we still add parenthesis for ease of understanding
                    if (innerBinaryExpression.OperatorType is ExpressionType.AndAlso or ExpressionType.And
                        && outerExpression is SqlBinaryExpression { OperatorType: ExpressionType.OrElse or ExpressionType.Or })
                    {
                        return true;
                    }

                    // If the provider defined precedence for the two expression, use that
                    if (TryGetOperatorInfo(outerExpression, out outerPrecedence, out var isOuterAssociative)
                        && TryGetOperatorInfo(innerExpression, out innerPrecedence, out _))
                    {
                        return outerPrecedence.CompareTo(innerPrecedence) switch
                        {
                            > 0 => true,
                            < 0 => false,

                            // If both operators have the same precedence, add parentheses unless they're the same operator, and
                            // that operator is associative (e.g. a + b + c)
                            0 => outerExpression is not SqlBinaryExpression outerBinary
                                || outerBinary.OperatorType != innerBinaryExpression.OperatorType
                                || !isOuterAssociative
                                // Arithmetic operators on floating points aren't associative, because of rounding errors.
                                || outerExpression.Type == typeof(float)
                                || outerExpression.Type == typeof(double)
                                || innerExpression.Type == typeof(float)
                                || innerExpression.Type == typeof(double)
                        };
                    }

                    // Even if the provider doesn't define precedence, assume that AND has less precedence than any other binary operator
                    // except for OR. This is universal, was our behavior before introducing provider precedence and removes the need for many
                    // parentheses. Do the same for OR (though here we add parentheses around inner AND just for readability).
                    if (outerExpression is SqlBinaryExpression outerBinary2)
                    {
                        if (outerBinary2.OperatorType == ExpressionType.AndAlso)
                        {
                            return innerBinaryExpression.OperatorType == ExpressionType.OrElse;
                        }

                        if (outerBinary2.OperatorType == ExpressionType.OrElse)
                        {
                            // Precedence-wise AND is above OR but we still add parentheses for ease of understanding
                            return innerBinaryExpression.OperatorType == ExpressionType.AndAlso;
                        }
                    }

                    // Otherwise always parenthesize for safety
                    return true;
                }

            default:
                return false;
        }
    }

    protected virtual bool TryGetOperatorInfo(SqlExpression expression, out int precedence, out bool isAssociative)
    {
        (precedence, isAssociative) = (default, default);
        return false;
    }

}