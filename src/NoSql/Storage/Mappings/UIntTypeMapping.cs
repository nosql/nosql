using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class UIntTypeMapping : TypeMapping
{
    public UIntTypeMapping(string storeType) : base(typeof(uint), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => (uint)reader.GetInt64(ordinal);

}
