using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class ByteTypeMapping : TypeMapping
{
    public ByteTypeMapping(string storeType) : base(typeof(byte), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetByte(ordinal);
}
