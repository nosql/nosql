using NoSql.Storage;
using System.Linq.Expressions;

namespace NoSql.Query.Expressions;

public class SqlBinaryExpression : SqlExpression
{
    public SqlBinaryExpression(Type type, TypeMapping? typeMapping, ExpressionType operatorType, SqlExpression left, SqlExpression right) : base(type, typeMapping)
    {
        OperatorType = operatorType;
        Left = left;
        Right = right;
    }

    public SqlBinaryExpression(TypeMapping typeMapping, ExpressionType operatorType, SqlExpression left, SqlExpression right) : base(typeMapping.ClrType, typeMapping)
    {
        OperatorType = operatorType;
        Left = left;
        Right = right;
    }

    public ExpressionType OperatorType { get; }

    public SqlExpression Left { get; }
    public SqlExpression Right { get; }
}
