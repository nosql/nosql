using System.Data.Common;
using System.Globalization;
using System.Text;

namespace NoSql.Storage.Mappings;

public class ByteArrayTypeMapping : TypeMapping
{
    public ByteArrayTypeMapping(string storeType) : base(typeof(bool), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
    {
        using var stream = reader.GetStream(ordinal);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    ///     Generates the SQL representation of a literal value.
    /// </summary>
    /// <param name="value">The literal value.</param>
    /// <returns>
    ///     The generated string.
    /// </returns>
    protected override string GenerateNonNullSqlLiteral(object value)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("X'");

        foreach (var @byte in (byte[])value)
        {
            stringBuilder.Append(@byte.ToString("X2", CultureInfo.InvariantCulture));
        }

        stringBuilder.Append('\'');
        return stringBuilder.ToString();
    }
}
