using System.Data.Common;
using System.Globalization;

namespace NoSql.Storage.Mappings;

public class FloatTypeMapping : TypeMapping
{
    public FloatTypeMapping(string storeType) : base(typeof(float), storeType) { }
    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetFloat(ordinal);

    protected override string GenerateNonNullSqlLiteral(object value)
        => Convert.ToSingle(value).ToString("R", CultureInfo.InvariantCulture);

}
