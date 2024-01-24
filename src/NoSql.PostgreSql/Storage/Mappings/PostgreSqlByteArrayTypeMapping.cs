using NoSql.Storage.Mappings;
using System.Globalization;
using System.Text;

namespace NoSql.PostgreSql.Storage.Mappings;

public class PostgreSqlByteArrayTypeMapping : ByteArrayTypeMapping
{
    public PostgreSqlByteArrayTypeMapping() : base("bytea")
    {
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
        var bytea = (byte[])value;

        var builder = new StringBuilder(bytea.Length * 2 + 6);

        builder.Append("BYTEA E'\\\\x");
        foreach (var b in bytea)
        {
            builder.Append(b.ToString("X2", CultureInfo.InvariantCulture));
        }

        builder.Append('\'');

        return builder.ToString();
    }
}
