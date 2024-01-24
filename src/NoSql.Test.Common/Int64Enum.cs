namespace NoSql.Test;

public enum Int64Enum : long
{
    A = long.MaxValue,
    B = 0,
    C = long.MaxValue - 2,
}

public enum Int16Enum : short
{
    A = short.MinValue,
    B = 0,
    C = short.MaxValue - 1,
}

public enum Int8Enum : byte
{
    A = 0,
    B = 1,
    C = byte.MaxValue - 1,
}