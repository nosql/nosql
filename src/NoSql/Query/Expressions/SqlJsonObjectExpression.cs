using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlJsonObjectExpression : SqlExpression
{
    public SqlJsonObjectExpression(TypeMapping typeMapping, IDictionary<string, SqlExpression> properties) : base(typeMapping.ClrType, typeMapping)
    {
        Properties = properties;
    }

    public SqlJsonObjectExpression(TypeMapping typeMapping) : base(typeMapping)
    {
        Properties = new Dictionary<string, SqlExpression>();
    }

    private SqlJsonObjectExpression() : base(typeof(object), null!)
    {
        Properties = new Dictionary<string, SqlExpression>();
    }

    public IDictionary<string, SqlExpression> Properties { get; }

    public void Merge(SqlJsonObjectExpression patch)
    {
        Merge(this, patch.Properties);

        static void Merge(SqlJsonObjectExpression source, IDictionary<string, SqlExpression> patch)
        {
            var properties = source.Properties;
            foreach (var property in patch)
            {
                if (properties.ContainsKey(property.Key))
                {
                    var propertyValue = properties[property.Key];
                    if (propertyValue is SqlJsonObjectExpression subPatch)
                    {
                        Merge(subPatch, subPatch.Properties);
                    }
                    else
                    {
                        properties[property.Key] = property.Value;
                    }
                }
                else
                {
                    properties.Add(property.Key, property.Value);
                }
            }
        }
    }

    public void Merge(string[] path, SqlExpression value)
    {
        IDictionary<string, SqlExpression> properties = Properties;
        for (int i = 0; i < path.Length; i++)
        {
            string segment = path[i];
            if (i == path.Length - 1)
            {
                if (properties.ContainsKey(segment))
                {
                    properties[segment] = value;
                }
                else
                {
                    properties.Add(segment, value);
                }
            }
            else
            {
                if (properties.ContainsKey(segment))
                {
                    if (properties[segment] is SqlJsonObjectExpression patch)
                    {
                        properties = patch.Properties;
                    }
                }
                else
                {
                    var patch = new SqlJsonObjectExpression();
                    properties.Add(segment, patch);
                    properties = patch.Properties;
                }
            }
        }
    }

}
