using NoSql.Query.Parser;
using System.Globalization;

namespace NoSql;

public class NoSqlException : Exception
{
    public NoSqlException() { }

    public NoSqlException(string message) : base(message) { }
}

public class NoSqlTranslateException : NoSqlException
{
    public NoSqlTranslateException() { }

    public NoSqlTranslateException(string message) : base(message) { }
}

public sealed class NoSqlSyntaxParseException : NoSqlException
{
    public NoSqlSyntaxParseException(string message, int position)
        : base(message)
    {
        Position = position;
    }

    public int Position { get; }

    public override string ToString()
    {
        return string.Format(CultureInfo.CurrentCulture, ExceptionStrings.ParseExceptionFormat, Message, Position);
    }
}
