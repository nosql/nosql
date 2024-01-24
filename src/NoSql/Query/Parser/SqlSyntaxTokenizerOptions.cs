namespace NoSql.Query.Parser;

public class SqlSyntaxTokenizerOptions
{
    public readonly Dictionary<string, SqlSyntaxTokenKind> TokenAliases = new();

    public void ApplyODataTokenAliases()
    {
        TokenAliases.Add("eq", SqlSyntaxTokenKind.EqualsEquals);
        TokenAliases.Add("ne", SqlSyntaxTokenKind.ExclamationEquals);
        TokenAliases.Add("lt", SqlSyntaxTokenKind.LessThan);
        TokenAliases.Add("le", SqlSyntaxTokenKind.LessThanOrEquals);
        TokenAliases.Add("gt", SqlSyntaxTokenKind.GreaterThan);
        TokenAliases.Add("ge", SqlSyntaxTokenKind.GreaterThanOrEquals);
        TokenAliases.Add("and", SqlSyntaxTokenKind.AmpersandAmpersand);
        TokenAliases.Add("or", SqlSyntaxTokenKind.BarBar);
        TokenAliases.Add("not", SqlSyntaxTokenKind.Exclamation);
        TokenAliases.Add("mod", SqlSyntaxTokenKind.Percent);
    }
}
