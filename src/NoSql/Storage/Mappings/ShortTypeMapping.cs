using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class ShortTypeMapping : TypeMapping
{
    public ShortTypeMapping(string storeType) : base(typeof(short), storeType) { }
    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetInt16(ordinal);

}
