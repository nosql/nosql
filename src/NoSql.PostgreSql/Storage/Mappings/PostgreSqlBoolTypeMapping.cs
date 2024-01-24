using NoSql.Storage.Mappings;

namespace NoSql.PostgreSql.Storage.Mappings;

public class PostgreSqlBoolTypeMapping : BoolTypeMapping
{
    public PostgreSqlBoolTypeMapping() : base("boolean")
    {
    }

    protected override string GenerateNonNullSqlLiteral(object value) => (bool)value ? "TRUE" : "FALSE";
}
