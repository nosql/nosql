using NoSql.Storage;

namespace NoSql.Query.Expressions;

public class SqlFunctionExpression : SqlExpression
{
    public SqlFunctionExpression(Type type,
                                 TypeMapping? typeMapping,
                                 string name,
                                 params SqlExpression[] arguments) : base(type, typeMapping)
    {
        Name = name;
        Arguments = arguments;
    }

    public SqlFunctionExpression(TypeMapping typeMapping,
                                 string name,
                                 params SqlExpression[] arguments) : base(typeMapping.ClrType, typeMapping)
    {
        Name = name;
        Arguments = arguments;
    }

    public string Name { get; }
    public SqlExpression[] Arguments { get; set; }

    public static readonly SqlFunctionExpression CountAll = new(typeof(int), null!, CountFunctionName, new SqlFragmentExpression("*"));

    public const string CountFunctionName = "COUNT";
    public const string SumFunctionName = "SUM";
    public const string MaxFunctionName = "MAX";
    public const string MinFunctionName = "MIN";
    public const string AvgFunctionName = "AVG";
}