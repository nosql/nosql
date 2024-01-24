using System.Globalization;
using System.Runtime.CompilerServices;

namespace NoSql.Query.Parser;

public class SqlSyntaxTokenizer
{
    public static readonly char NumberDecimalSeparator = '.';

    private readonly SqlSyntaxTokenizerOptions _options;
    private readonly string _text;
    private readonly int _textLen;
    private int _textPos;
    private char _ch;

    public SqlSyntaxToken CurrentToken;

    public SqlSyntaxTokenizer(string text, SqlSyntaxTokenizerOptions? options = null)
    {
        _options = options ?? new SqlSyntaxTokenizerOptions();
        _text = text;
        _textLen = _text.Length;
        _textPos = 0;
        _ch = _textPos < _textLen ? _text[0] : '\0';
        NextToken();
    }

    private void NextChar()
    {
        if (_textPos < _textLen) _textPos++;
        _ch = _textPos < _textLen ? _text[_textPos] : '\0';
    }

    public void NextToken()
    {
        while (char.IsWhiteSpace(_ch))
            NextChar();

        SqlSyntaxTokenKind tokenKind;
        int tokenPos = _textPos;

        switch (_ch)
        {
            case '!':
                NextChar();
                if (_ch == '=')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.ExclamationEquals;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Exclamation;
                }
                break;

            case '%':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Percent;
                break;

            case '&':
                NextChar();
                if (_ch == '&')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.AmpersandAmpersand;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Ampersand;
                }
                break;

            case '(':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.OpenParen;
                break;

            case ')':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.CloseParen;
                break;

            case '{':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.OpenBrace;
                break;

            case '}':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.CloseBrace;
                break;

            case '*':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Asterisk;
                break;

            case '+':
                NextChar();
                if (_ch == '+')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.PlusPlus;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Plus;
                }
                break;

            case ',':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Comma;
                break;

            case ';':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Semicolons;
                break;

            case '-':
                NextChar();
                if (_ch == '-')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.MinusMinus;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Minus;
                }
                break;

            case '.':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Dot;
                break;

            case '/':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Slash;
                break;

            case ':':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.Colon;
                break;

            case '<':
                NextChar();
                if (_ch == '=')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.LessThanOrEquals;
                }
                else if (_ch == '>')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.LessGreater;
                }
                else if (_ch == '<')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.LessThanLessThan;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.LessThan;
                }
                break;

            case '=':
                NextChar();
                if (_ch == '=')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.EqualsEquals;
                }
                else if (_ch == '>')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.EqualsGreaterThan;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Equals;
                }
                break;

            case '>':
                NextChar();
                if (_ch == '=')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.GreaterThanOrEquals;
                }
                else if (_ch == '>')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.GreaterThanGreaterThan;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.GreaterThan;
                }
                break;

            case '?':
                NextChar();
                if (_ch == '?')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.QuestionQuestion;
                }
                else if (_ch == '.')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.DotDot;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Question;
                }
                break;

            case '[':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.OpenBracket;
                break;

            case ']':
                NextChar();
                tokenKind = SqlSyntaxTokenKind.CloseBracket;
                break;

            case '|':
                NextChar();
                if (_ch == '|')
                {
                    NextChar();
                    tokenKind = SqlSyntaxTokenKind.BarBar;
                }
                else
                {
                    tokenKind = SqlSyntaxTokenKind.Bar;
                }
                break;

            case '"':
            case '\'':
                char quote = _ch;
                do
                {
                    bool escaped;

                    do
                    {
                        escaped = false;
                        NextChar();

                        if (_ch == '\\')
                        {
                            escaped = true;
                            if (_textPos < _textLen) NextChar();
                        }
                    }
                    while (_textPos < _textLen && (_ch != quote || escaped));

                    if (_textPos == _textLen)
                        throw Error(_textPos, ExceptionStrings.UnterminatedStringLiteral);

                    NextChar();
                } while (_ch == quote);

                tokenKind = SqlSyntaxTokenKind.StringLiteral;
                break;

            default:
                if (char.IsLetter(_ch) || _ch == '@' || _ch == '_' || _ch == '$' || _ch == '^' || _ch == '~')
                {
                    do
                    {
                        NextChar();
                    } while (char.IsLetterOrDigit(_ch) || _ch == '_');
                    tokenKind = SqlSyntaxTokenKind.Identifier;
                    break;
                }

                if (char.IsDigit(_ch))
                {
                    tokenKind = SqlSyntaxTokenKind.IntegerLiteral;
                    do
                    {
                        NextChar();
                    } while (char.IsDigit(_ch));

                    bool hexInteger = false;
                    if (_ch == 'X' || _ch == 'x')
                    {
                        NextChar();
                        ValidateHexChar();
                        do
                        {
                            NextChar();
                        } while (_ch.IsHexChar());

                        hexInteger = true;
                    }

                    if (_ch == 'U' || _ch == 'u')
                    {
                        NextChar();
                        if (_ch == 'L' || _ch == 'l') NextChar();
                        ValidateExpression();
                        break;
                    }

                    if (_ch == 'L' || _ch == 'l')
                    {
                        NextChar();
                        ValidateExpression();
                        break;
                    }

                    if (hexInteger)
                        break;

                    if (_ch == NumberDecimalSeparator)
                    {
                        tokenKind = SqlSyntaxTokenKind.RealLiteral;
                        NextChar();
                        ValidateDigit();
                        do
                        {
                            NextChar();
                        } while (char.IsDigit(_ch));
                    }

                    if (_ch == 'E' || _ch == 'e')
                    {
                        tokenKind = SqlSyntaxTokenKind.RealLiteral;
                        NextChar();
                        if (_ch == '+' || _ch == '-') NextChar();
                        ValidateDigit();
                        do
                        {
                            NextChar();
                        } while (char.IsDigit(_ch));
                    }

                    if (_ch == 'F' || _ch == 'f' ||
                        _ch == 'D' || _ch == 'd' ||
                        _ch == 'M' || _ch == 'm')
                    {
                        tokenKind = SqlSyntaxTokenKind.RealLiteral;
                        NextChar();
                    }
                    break;
                }

                if (_textPos == _textLen)
                {
                    tokenKind = SqlSyntaxTokenKind.End;
                    break;
                }

                throw Error(_textPos, ExceptionStrings.InvalidCharacter, _ch);
        }

        var current = _text[tokenPos.._textPos];
        var kind = GetAliasedTokenKind(tokenKind, current);
        CurrentToken = new(kind, current, tokenPos);
    }

    public void ValidateToken(SqlSyntaxTokenKind t, string? errorMessage = null)
    {
        if (CurrentToken.Kind != t)
            throw Error(errorMessage ?? string.Format(ExceptionStrings.TokenExpected, t.ToString()));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateExpression()
    {
        if (char.IsLetterOrDigit(_ch)) throw Error(_textPos, ExceptionStrings.ExpressionExpected);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateDigit()
    {
        if (!char.IsDigit(_ch)) throw Error(_textPos, ExceptionStrings.DigitExpected);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateHexChar()
    {
        if (!_ch.IsHexChar()) throw Error(_textPos, ExceptionStrings.HexCharExpected);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private SqlSyntaxTokenKind GetAliasedTokenKind(SqlSyntaxTokenKind t, string alias)
    {
        return t == SqlSyntaxTokenKind.Identifier &&
            !string.IsNullOrEmpty(alias) &&
            _options.TokenAliases.TryGetValue(alias, out SqlSyntaxTokenKind id) ? id : t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private NoSqlSyntaxParseException Error(string format, params object[] args) => new(string.Format(CultureInfo.CurrentCulture, format, args), CurrentToken.Pos);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static NoSqlSyntaxParseException Error(int pos, string format, params object[] args) => new(string.Format(CultureInfo.CurrentCulture, format, args), pos);

}
