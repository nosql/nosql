using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class LongTypeMapping : TypeMapping
{
    public LongTypeMapping(string storeType) : base(typeof(long), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetInt64(ordinal);

}
