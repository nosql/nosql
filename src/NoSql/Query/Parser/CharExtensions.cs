namespace NoSql.Query.Parser;

internal static class CharExtensions
{
    public static bool IsHexChar(this char c)
    {
        if (char.IsDigit(c))
            return true;

        if (c <= '\x007f')
        {
            c |= (char)0x20;
            return c >= 'a' && c <= 'f';
        }

        return false;
    }
}
