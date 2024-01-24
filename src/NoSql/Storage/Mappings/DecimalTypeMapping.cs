using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class DecimalTypeMapping : TypeMapping
{
    private const string DecimalFormatConst = "{0:0.0###########################}";

    public DecimalTypeMapping(string storeType) : base(typeof(decimal), storeType) { }

    protected override string SqlLiteralFormatString => DecimalFormatConst;

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal) => reader.GetDecimal(ordinal);

}
