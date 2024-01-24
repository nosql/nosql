using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class UShortTypeMapping : TypeMapping
{
    public UShortTypeMapping(string storeType) : base(typeof(ushort), storeType) { }
    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => (ushort)reader.GetInt32(ordinal);
}
