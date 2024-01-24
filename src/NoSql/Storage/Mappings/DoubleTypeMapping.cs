using System.Data.Common;
using System.Globalization;

namespace NoSql.Storage.Mappings;

public class DoubleTypeMapping : TypeMapping
{
    public DoubleTypeMapping(string storeType) : base(typeof(double), storeType) { }
    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetDouble(ordinal);

    protected override string GenerateNonNullSqlLiteral(object value)
    {
        var doubleValue = Convert.ToDouble(value);
        var literal = doubleValue.ToString("G17", CultureInfo.InvariantCulture);

        return !literal.Contains('E')
            && !literal.Contains('e')
            && !literal.Contains('.')
            && !double.IsNaN(doubleValue)
            && !double.IsInfinity(doubleValue)
                ? literal + ".0"
                : literal;
    }
}
