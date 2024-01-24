using System.Data.Common;

namespace NoSql.Storage.Mappings;

public class CharTypeMapping : TypeMapping
{
    public CharTypeMapping(string storeType) : base(typeof(char), storeType) { }

    public override object ReadNonNullFromDataReader(DbDataReader reader, int ordinal)
    {
        return reader.GetString(ordinal)[0];
    }

    protected override string GenerateNonNullSqlLiteral(object value)
    {
        // NB: We can get Int32 values here too due to compiler-introduced convert nodes
        var charValue = Convert.ToChar(value);
        if (charValue == '\'')
        {
            return "''''";
        }

        return "'" + charValue + "'";
    }
}