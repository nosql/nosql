using NoSql.Query;
using NoSql.Query.Expressions;

namespace NoSql.Sqlite.Query;

public class SqliteSqlGenerator : SqlGenerator
{
    protected override void VisitJsonBuildArray(SqlJsonBuildArrayExpression expression)
    {
        string column;
        if (expression.Subquery.Projections is SqlProjectionExpression projection)
        {
            column = projection.Alias!;
        }
        else if (expression.Subquery.Projections is SqlProjectionListExpression projectionList && projectionList.ProjectionList.Length == 1)
        {
            column = projectionList.ProjectionList[0].Alias!;
        }
        else
        {
            throw new Exception();
        }

        SqlBuilder.Append('(');
        SqlBuilder.Append($"SELECT json_group_array(json({column})) FROM (");
        VisitSelect(expression.Subquery);
        SqlBuilder.Append("))");

    }

    protected override void VisitJsonObject(SqlJsonObjectExpression expression)
    {
        SqlBuilder.Append($"json_object(");
        int i = 0;
        foreach (var property in expression.Properties)
        {
            SqlBuilder.Append($"'{property.Key}', ");

            if (property.Value is SqlColumnExpression column && column.TypeMapping!.IsJsonType)
            {
                SqlBuilder.Append($"json(");

                VisitColumn(column);

                SqlBuilder.Append(")");
            }
            else
            {
                Visit(property.Value);
            }

            i++;
            if (i < expression.Properties.Count)
            {
                SqlBuilder.Append(',');
            }
        }

        SqlBuilder.Append(")");
    }

    protected override void VisitJsonExtract(SqlJsonExtractExpression expression)
    {
        SqlBuilder.Append("json_extract(");
        Visit(expression.Column);

        SqlBuilder.Append(",'");

        GenerateJsonPath(expression.Path);

        SqlBuilder.Append("')");
    }

    protected override void VisitJsonSet(SqlJsonSetExpression expression)
    {
        SqlBuilder.Append($"json_set(");

        Visit(expression.Expression);

        SqlBuilder.Append(",'");
        GenerateJsonPath(expression.Path);
        SqlBuilder.Append("',");

        Visit(expression.Value);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonMerge(SqlJsonMergeExpression expression)
    {
        SqlBuilder.Append($"json_patch(");
        Visit(expression.Expression);
        SqlBuilder.Append(",");
        Visit(expression.Value);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonArrayEach(SqlJsonArrayEachExpression expression)
    {
        SqlBuilder.Append("json_each(");
        Visit(expression.Expression);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonArrayLength(SqlJsonArrayLengthExpression expression)
    {
        SqlBuilder.Append("json_array_length(");
        Visit(expression.Expression);
        SqlBuilder.Append(")");
    }

    protected override void VisitLimitOffset(int? offset, int? limit) => base.VisitLimitOffset(offset, limit ?? int.MaxValue);
}
