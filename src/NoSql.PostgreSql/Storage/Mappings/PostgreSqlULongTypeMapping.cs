using NoSql.Storage;
using System.Data.Common;

namespace NoSql.PostgreSql.Storage.Mappings;

public class PostgreSqlULongTypeMapping : TypeMapping
{
    public PostgreSqlULongTypeMapping() : base(typeof(ulong), "xid8")
    {
    }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
    {
        return (ulong)reader.GetInt64(ordinal);
    }
}
