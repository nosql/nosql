namespace NoSql.Query.Parser;

public readonly struct SqlSyntaxToken
{
    public SqlSyntaxToken(SqlSyntaxTokenKind kind, string text, int pos)
    {
        Kind = kind;
        Text = text;
        Pos = pos;
    }

    public readonly SqlSyntaxTokenKind Kind;
    public readonly string Text;
    public readonly int Pos;
}
