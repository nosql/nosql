using NoSql.Extensions;
using System.Data.Common;
using System.Globalization;

namespace NoSql.Storage;

public abstract class TypeMapping
{
    public TypeMapping(Type clrType, string storeType)
    {
        ClrType = clrType;
        StoreType = storeType;
    }

    public virtual string GenerateSqlLiteral(object? value)
    {
        value = NormalizeEnumValue(value);

        return value == null ? "NULL" : GenerateNonNullSqlLiteral(value);
    }

    public string StoreType { get; }
    public virtual Type ClrType { get; }
    public virtual bool IsJsonType { get; }

    public object? ReadFromDataReader(DbDataReader reader, int ordinal)
    {
        try
        {
            if (reader.IsDBNull(ordinal))
                return null;

            return ReadNonNullFromDataReader(reader, ordinal);
        }
        catch (ObjectDisposedException)
        {
            return null;
        }
    }

    public abstract object ReadNonNullFromDataReader(DbDataReader reader, int ordinal);

    protected virtual string SqlLiteralFormatString => "{0}";

    protected virtual string GenerateNonNullSqlLiteral(object value) => string.Format(CultureInfo.InvariantCulture, SqlLiteralFormatString, value);

    protected object? NormalizeEnumValue(object? value)
    {
        // When Enum column is compared to constant the C# compiler put a constant of integer there
        // In some unknown cases for parameter we also see integer value.
        // So if CLR type is enum we need to convert integer value to enum value
        if (value?.GetType().IsInteger() == true && ClrType.UnwrapNullableType().IsEnum)
        {
            return Enum.ToObject(ClrType.UnwrapNullableType(), value);
        }

        // When Enum is cast manually our logic of removing implicit convert gives us enum value here
        // So if CLR type is integer we need to convert enum value to integer value
        if (value?.GetType().IsEnum == true && ClrType.UnwrapNullableType().IsInteger())
        {
            return Convert.ChangeType(value, ClrType);
        }

        return value;
    }
}