using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class BoolTypeMapping : TypeMapping
{
    public BoolTypeMapping(string storeType) : base(typeof(bool), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetBoolean(ordinal);

    protected override string GenerateNonNullSqlLiteral(object value) => (bool)value ? "1" : "0";
}
