namespace NoSql.Query.Parser;

public enum SqlSyntaxTokenKind
{
    Unknown,
    End,


    /// <summary> Identifier </summary>
    Identifier,

    /// <summary> String Literal </summary>
    StringLiteral,

    /// <summary> Integer Literal </summary>
    IntegerLiteral,

    /// <summary> Real Literal </summary>
    RealLiteral,

    /// <summary> ! </summary>
    Exclamation,
    /// <summary> != </summary>
    ExclamationEquals,

    /// <summary> $ </summary>
    Dollar,

    /// <summary> % </summary>
    Percent,

    /// <summary> ( </summary>
    OpenParen,
    /// <summary> ) </summary>
    CloseParen,

    /// <summary> [ </summary>
    OpenBracket,
    /// <summary> ] </summary>
    CloseBracket,

    /// <summary> { </summary>
    OpenBrace,
    /// <summary> } </summary>
    CloseBrace,

    /// <summary> * </summary>
    Asterisk,

    /// <summary> + </summary>
    Plus,
    /// <summary> ++ </summary>
    PlusPlus,

    /// <summary> - </summary>
    Minus,
    /// <summary> -- </summary>
    MinusMinus,

    /// <summary> &amp; </summary>
    Ampersand,
    /// <summary> &amp;&amp; </summary>
    AmpersandAmpersand,

    /// <summary> | </summary>
    Bar,
    /// <summary> || </summary>
    BarBar,

    /// <summary> ? </summary>
    Question,
    /// <summary> ?? </summary>
    QuestionQuestion,

    /// <summary> , </summary>
    Comma,
    /// <summary> ; </summary>
    Semicolons,

    /// <summary> . </summary>
    Dot,
    /// <summary> .. </summary>
    DotDot,

    /// <summary> = </summary>
    Equals,
    /// <summary> == </summary>
    EqualsEquals,
    /// <summary> => </summary>
    EqualsGreaterThan,

    /// <summary> / </summary>
    Slash,

    /// <summary> : </summary>
    Colon,

    /// <summary> &lt; </summary>
    LessThan,
    /// <summary> &lt;= </summary>
    LessThanOrEquals,
    /// <summary> &lt;> </summary>
    LessGreater,
    /// <summary> &lt;&lt; </summary>
    LessThanLessThan,

    /// <summary> > </summary>
    GreaterThan,
    /// <summary> >= </summary>
    GreaterThanOrEquals,
    /// <summary> >> </summary>
    GreaterThanGreaterThan,
}