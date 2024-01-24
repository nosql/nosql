using NoSql.Query;
using NoSql.Query.Expressions;

namespace NoSql.PostgreSql.Query;

public class PostgreSqlGenerator : SqlGenerator
{
    protected override void VisitProjection(SqlProjectionExpression node)
    {
        Visit(node.Expression);

        if (node.Alias != null)
        {
            SqlBuilder.Append($" AS \"{node.Alias}\"");
        }
    }

    protected override void VisitJsonBuildArray(SqlJsonBuildArrayExpression expression)
    {
        SqlBuilder.Append("array_to_json(ARRAY(");
        VisitSelect(expression.Subquery);
        SqlBuilder.Append("))::jsonb");
    }

    protected override void VisitJsonObject(SqlJsonObjectExpression expression)
    {
        SqlBuilder.Append($"jsonb_build_object(");
        int i = 0;
        foreach (var property in expression.Properties)
        {
            SqlBuilder.Append($"'{property.Key}', ");

            if (property.Value is SqlColumnExpression column && column.TypeMapping!.IsJsonType)
            {
                SqlBuilder.Append($"to_jsonb(");

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

    protected override void VisitJsonSet(SqlJsonSetExpression node)
    {
        SqlBuilder.Append($"jsonb_set(");

        Visit(node.Expression);

        SqlBuilder.Append(", '{");

        for (int i = 0; i < node.Path.Length; i++)
        {
            var segment = node.Path[i];
            if (segment.PropertyName == null)
            {
                Visit(segment.ArrayIndex!);
            }
            else
            {
                SqlBuilder.Append(segment.PropertyName);
            }
            if (i < node.Path.Length - 1)
                SqlBuilder.Append(",");
        }

        SqlBuilder.Append("}', to_jsonb(");

        if (node.Value is SqlConstantExpression constant && constant.Value == null)
        {
            SqlBuilder.Append("'null'::json");
        }
        else
        {
            Visit(node.Value);

            if (CastToText(node.Value.Type))
            {
                SqlBuilder.Append($"::text");
            }
        }
        SqlBuilder.Append("), true)");
    }

    //// Concatenates two values. Concatenating two arrays generates an array containing all the elements of each input. Concatenating two objects generates an object containing the union of their keys, taking the second object's value when there are duplicate keys. All other cases are treated by converting a non-array input into a single-element array, and then proceeding as for two arrays. Does not operate recursively: only the top-level array or object structure is merged
    //// Does not operate recursively: only the top-level array or object structure is merged
    //protected override void VisitJsonPatch(SqlJsonPatchExpression expression)
    //{
    //    Visit(expression.Column);
    //    SqlBuilder.Append(" || ");
    //    VisitJsonObject(expression.Value);
    //}

    protected override void VisitJsonExtract(SqlJsonExtractExpression node)
    {
        SqlBuilder.Append("jsonb_extract_path_text(");

        Visit(node.Column);

        if (node.Path.Length > 0)
        {
            SqlBuilder.Append(",'");

            for (int i = 0; i < node.Path.Length; i++)
            {
                var segment = node.Path[i];
                if (segment.PropertyName == null)
                {
                    Visit(segment.ArrayIndex!);
                }
                else
                {
                    SqlBuilder.Append(segment.PropertyName);
                }
                if (i < node.Path.Length - 1)
                    SqlBuilder.Append("','");
            }

            SqlBuilder.Append("')");

        }
        else
        {
            SqlBuilder.Append(")");
        }

        if (node.TypeMapping != null)
        {
            SqlBuilder.Append($"::{node.TypeMapping.StoreType}");
        }
    }

    private static bool CastToText(Type type) => type == typeof(string) || type == typeof(char) || type == typeof(DateTime);

    protected override void VisitJsonArrayEach(SqlJsonArrayEachExpression node)
    {
        SqlBuilder.Append("jsonb_array_elements(");
        Visit(node.Expression);
        SqlBuilder.Append(")");
    }

    protected override void VisitJsonArrayEachItem(SqlJsonArrayEachItemExpression node)
    {
        SqlBuilder.Append("value");
        SqlBuilder.Append($"::{node.TypeMapping!.StoreType}");
    }

    protected override void VisitJsonArrayLength(SqlJsonArrayLengthExpression expression)
    {
        SqlBuilder.Append("jsonb_array_length(");
        Visit(expression.Expression);
        SqlBuilder.Append(")");
    }
}
