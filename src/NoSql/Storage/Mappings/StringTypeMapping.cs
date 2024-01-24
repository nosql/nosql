using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class StringTypeMapping : TypeMapping
{
    public StringTypeMapping(string storeType) : base(typeof(string), storeType) { }
    protected virtual string EscapeSqlLiteral(string literal) => literal.Replace("'", "''");
    protected override string GenerateNonNullSqlLiteral(object value) => $"'{EscapeSqlLiteral((string)value)}'";

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetString(ordinal);

}
