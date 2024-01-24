using NoSql.Query.Expressions;
using NoSql.Storage;

namespace NoSql.Query.Parser;

public class JsonPathParser
{
    private readonly SqlSyntaxTokenizer _lexer;
    private readonly TypeMapping _typeMapping;

    public JsonPathParser(string expression, TypeMapping typeMapping)
    {
        if (expression == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(expression));
        }

        _lexer = new SqlSyntaxTokenizer(expression);
        _typeMapping = typeMapping;
    }

    public SqlExpression Parse()
    {
        PathSegment[]? segments = null;
        for (; ; )
        {
            segments = ParseExpression(segments);

            if (_lexer.CurrentToken.Kind == SqlSyntaxTokenKind.End)
                break;
        }

        if (segments == null || segments.Length == 0)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(segments));
        }

        if (segments[0].PropertyName == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(PathSegment.PropertyName));
        }

        if (segments.Length == 1)
        {
            return new SqlColumnExpression(_typeMapping, segments[0].PropertyName!);
        }
        else
        {

            PathSegment[] newpath = new PathSegment[segments.Length - 1];
            Array.Copy(segments, 1, newpath, 0, newpath.Length);
            var column = new SqlColumnExpression(typeof(object), null, segments[0].PropertyName!);
            return new SqlJsonExtractExpression(_typeMapping, column, newpath);
        }

    }

    protected virtual PathSegment[] ParseExpression(PathSegment[]? previous)
    {
        var token = _lexer.CurrentToken;

        switch (token.Kind)
        {
            case SqlSyntaxTokenKind.Identifier:
                {
                    PathSegment[] path;
                    if (previous == null || previous.Length == 0)
                    {
                        path = new PathSegment[] { new PathSegment(token.Text) };
                    }
                    else
                    {
                        path = new PathSegment[previous.Length + 1];
                        Array.Copy(previous, path, previous.Length);
                        path[path.Length - 1] = new PathSegment(token.Text);
                    }

                    _lexer.NextToken();
                    return path;
                }
            case SqlSyntaxTokenKind.Dot:                  //  .
                {
                    if (previous == null)
                    {
                        ThrowHelper.ThrowParseException_ParseError(_lexer.CurrentToken.Pos);
                    }
                    _lexer.NextToken();
                    return previous;
                }
            case SqlSyntaxTokenKind.OpenBracket:          //  [
                {
                    _lexer.NextToken();
                    _lexer.ValidateToken(SqlSyntaxTokenKind.IntegerLiteral);

                    var index = new PathSegment(new SqlConstantExpression(typeof(int), null, int.Parse(_lexer.CurrentToken.Text)));
                    PathSegment[] path;
                    if (previous == null || previous.Length == 0)
                    {
                        path = new PathSegment[] { index };
                    }
                    else
                    {
                        path = new PathSegment[previous.Length + 1];
                        Array.Copy(previous, path, previous.Length);
                        path[path.Length - 1] = index;
                    }

                    _lexer.NextToken();
                    _lexer.ValidateToken(SqlSyntaxTokenKind.CloseBracket);
                    _lexer.NextToken();

                    return path;
                }
            default:
                {
                    ThrowHelper.ThrowParseException_ParseError(_lexer.CurrentToken.Pos);
                    return null;
                }
        }
    }

}
