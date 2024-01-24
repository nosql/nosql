using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class ULongTypeMapping : TypeMapping
{
    public ULongTypeMapping(string storeType) : base(typeof(ulong), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => (ulong)reader.GetInt64(ordinal);

    protected override string GenerateNonNullSqlLiteral(object value) => value.ToString()!;

}
