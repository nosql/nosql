using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class DateTimeTypeMapping : TypeMapping
{
    public DateTimeTypeMapping(string storeType) : base(typeof(DateTime), storeType)
    {
    }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
    {
        return reader.GetDateTime(ordinal);
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
        return $"'{(DateTime)value:yyyy-MM-dd HH:mm:ss.fff%K}'";
    }
}