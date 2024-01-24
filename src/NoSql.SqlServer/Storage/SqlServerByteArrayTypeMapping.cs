using NoSql.Storage.Mappings;
using System.Globalization;
using System.Text;

namespace NoSql.SqlServer.Storage;

public class SqlServerByteArrayTypeMapping : ByteArrayTypeMapping
{
    public SqlServerByteArrayTypeMapping() : base("varbinary(max)")
    {
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
        var builder = new StringBuilder();
        builder.Append("0x");

        foreach (var @byte in (byte[])value)
        {
            builder.Append(@byte.ToString("X2", CultureInfo.InvariantCulture));
        }

        return builder.ToString();
    }

}
