using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class SByteTypeMapping : TypeMapping
{
    public SByteTypeMapping(string storeType) : base(typeof(sbyte), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => (sbyte)reader.GetInt16(ordinal);

}
