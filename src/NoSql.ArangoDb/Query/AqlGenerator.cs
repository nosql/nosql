using NoSql.Extensions;
using NoSql.Query;
using NoSql.Query.Expressions;
using System.Linq.Expressions;

namespace NoSql.ArangoDb.Query;

public class AqlGenerator : SqlGenerator
{
    private static readonly Dictionary<ExpressionType, string> OperatorMap = new()
    {
        { ExpressionType.Equal, " == " },
        { ExpressionType.NotEqual, " != " },
        { ExpressionType.GreaterThan, " > " },
        { ExpressionType.GreaterThanOrEqual, " >= " },
        { ExpressionType.LessThan, " < " },
        { ExpressionType.LessThanOrEqual, " <= " },
        { ExpressionType.AndAlso, " && " },
        { ExpressionType.OrElse, " || " },
        { ExpressionType.Add, " + " },
        { ExpressionType.Subtract, " - " },
        { ExpressionType.Multiply, " * " },
        { ExpressionType.Divide, " / " },
        { ExpressionType.Modulo, " % " },
        { ExpressionType.And, " & " },
        { ExpressionType.Or, " | " }
    };

    private string _variable = "_p0";
    private int _variableNum = 0;

    protected override void VisitCast(SqlCastExpression expression)
    {
        if (expression.Type == typeof(string))
        {
            SqlBuilder.Append("TO_STRING(");
            Visit(expression.Expression);
            SqlBuilder.Append(")");
        }
        else if (expression.Type == typeof(bool))
        {
            SqlBuilder.Append("TO_BOOL(");
            Visit(expression.Expression);
            SqlBuilder.Append(")");
        }
        else if (expression.Type.IsNumeric())
        {
            SqlBuilder.Append("TO_NUMBER(");
            Visit(expression.Expression);
            SqlBuilder.Append(")");
        }
        else
        {
            Visit(expression.Expression);
        }
    }

    protected override void VisitTable(SqlTableExpression expression)
    {
        SqlBuilder.Append(expression.Name);
    }

    protected override void VisitColumn(SqlColumnExpression expression)
    {
        SqlBuilder.Append($"{_variable}[\"{expression.Name}\"]");
    }

    protected override void VisitJsonArrayLength(SqlJsonArrayLengthExpression expression)
    {
        SqlBuilder.Append("LENGTH(");
        Visit(expression.Expression);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonExtract(SqlJsonExtractExpression expression)
    {
        Visit(expression.Column);

        foreach (var segment in expression.Path)
        {
            if (segment.PropertyName != null)
            {
                SqlBuilder.Append($"[\"{segment.PropertyName}\"]");
            }
            else
            {
                SqlBuilder.Append("[");
                Visit(segment.ArrayIndex!);
                SqlBuilder.Append("]");
            }
        }
    }

    protected override void VisitJsonArrayEach(SqlJsonArrayEachExpression expression)
    {
        //if(expression.Expression is SqlColumnExpression column)
        //{
        //    var temp = _variable;
        //    _variable = $"_p{_variableNum - 2}";
        //    VisitColumn(column);
        //    _variable = temp;
        //}
        //else
        //{
        //    Visit(expression.Expression);
        //}

        Visit(expression.Expression);
    }

    protected override void VisitJsonArrayEachItem(SqlJsonArrayEachItemExpression expression)
    {
        SqlBuilder.Append(_variable);
    }

    private void VariableIncrease()
    {
        _variableNum++;
        _variable = $"_p{_variableNum}";
    }

    private void VariableReset()
    {
        _variable = "_p0";
    }

    protected override void VisitSelect(SqlSelectExpression expression)
    {
        if (expression.Table != null)
        {
            if (expression.Table is SqlJsonArrayEachExpression)
            {
                VariableIncrease();
            }

            var v = _variable;

            SqlBuilder.Append($"FOR {_variable} IN ");

            if (expression.Table is SqlJsonArrayEachExpression)
            {
                VariableReset();
            }

            Visit(expression.Table);

            if (expression.Table is SqlJsonArrayEachExpression)
            {
                _variable = v;
            }

            if (expression.Predicate != null)
            {
                SqlBuilder.Append(" FILTER ");
                Visit(expression.Predicate);
            }

            if (expression.Orderings != null && expression.Orderings.Length > 0)
            {
                SqlBuilder.Append(" SORT ");
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

            SqlBuilder.Append(" ");
        }

        SqlBuilder.Append("RETURN ");
        if (expression.Projections is SqlProjectionListExpression list && list.ProjectionList.Length == 0)
        {
            SqlBuilder.Append(_variable);
        }
        else if (expression.Projections is SqlProjectionExpression projectionExpression)
        {
            Visit(projectionExpression.Expression);
        }
        else
        {
            Visit(expression.Projections);
        }

        VariableReset();

    }

    protected override void VisitDelete(SqlDeleteExpression expression)
    {
        SqlBuilder.Append($"FOR {_variable} IN ");
        Visit(expression.Table);
        if(expression.Predicate != null)
        {
            SqlBuilder.Append(" FILTER ");
            Visit(expression.Predicate);
        }
        SqlBuilder.Append($" REMOVE {_variable} IN ");
        Visit(expression.Table);
    }

    protected override void VisitJsonSet(SqlJsonSetExpression expression)
    {
        SqlBuilder.Append("MERGE(");
        Visit(expression.Expression);
        SqlBuilder.Append(", {");

        for (int i = 0; i < expression.Path.Length; i++)
        {
            PathSegment segment = expression.Path[i];
            SqlBuilder.Append(segment.PropertyName);
            SqlBuilder.Append(":");
            if (i < expression.Path.Length - 1)
            {
                SqlBuilder.Append("{");
            }
        }
        Visit(expression.Value);
        for (int i = 0; i < expression.Path.Length; i++)
        {
            SqlBuilder.Append("}");
        }

        SqlBuilder.Append(")");
    }

    protected override void VisitExists(SqlExistsExpression expression)
    {
        SqlBuilder.Append("LENGTH(");
        Visit(expression.Expression);
        if (expression.IsNegated)
        {
            SqlBuilder.Append(") == 0");
        }
        else
        {
            SqlBuilder.Append(") > 0");
        }
    }

    protected override void VisitJsonMerge(SqlJsonMergeExpression expression)
    {
        SqlBuilder.Append("MERGE_RECURSIVE(");
        Visit(expression.Expression);
        SqlBuilder.Append(",");
        Visit(expression.Value);
        SqlBuilder.Append(")");
    }

    protected override void VisitUpdate(SqlUpdateExpression expression)
    {
        VariableIncrease();
        SqlBuilder.Append($"FOR {_variable} IN ");
        Visit(expression.Table);

        if (expression.Predicate != null)
        {
            SqlBuilder.Append(" FILTER ");
            Visit(expression.Predicate);
        }

        SqlBuilder.Append($" UPDATE {_variable} WITH {{");

        bool notMerge = expression.Setters.Any(x => x.Value is not SqlJsonMergeExpression);
        int index = 0;
        foreach (var setter in expression.Setters)
        {
            SqlBuilder.Append($"\"{setter.Column.Name}\":");
            if (setter.Value is SqlJsonMergeExpression mergeExpression)
            {
                Visit(mergeExpression.Value);
            }
            else
            {
                Visit(setter.Value);
            }

            index++;

            if (index < expression.Setters.Count)
            {
                SqlBuilder.Append(",");
            }
        }

        SqlBuilder.Append("} IN ");
        Visit(expression.Table);

        if (notMerge)
        {
            SqlBuilder.Append(" OPTIONS { mergeObjects: false }");
        }
    }

    protected override void VisitLimitOffset(int? offset, int? limit)
    {
        limit ??= int.MaxValue;

        if (offset.HasValue)
        {
            SqlBuilder.Append($"LIMIT {offset},{limit}");
        }
        else
        {
            SqlBuilder.Append($"LIMIT {limit}");
        }
    }

    protected override void VisitJsonBuildArray(SqlJsonBuildArrayExpression expression)
    {
        SqlBuilder.Append("(");
        Visit(expression.Subquery);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonObject(SqlJsonObjectExpression expression)
    {
        SqlBuilder.Append("{");

        int index = 0;
        foreach (var property in expression.Properties)
        {
            SqlBuilder.Append($"\"{property.Key}\":");
            Visit(property.Value);

            index++;
            if (index < expression.Properties.Count)
            {
                SqlBuilder.Append(",");
            }

        }
        SqlBuilder.Append("}");
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
                    SqlBuilder.Append("!(");
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

                    SqlBuilder.Append(" == null");
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

                    SqlBuilder.Append(" != null");
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

    protected override string GetOperator(SqlBinaryExpression expression) => OperatorMap[expression.OperatorType];

}