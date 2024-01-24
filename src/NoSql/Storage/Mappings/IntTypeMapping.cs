using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class IntTypeMapping : TypeMapping
{
    public IntTypeMapping(string storeType) : base(typeof(int), storeType) { }
    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetInt32(ordinal);

}
