using NoSql.Storage;
using System.Linq.Expressions;

namespace NoSql.Query.Expressions;

public class SqlUnaryExpression : SqlExpression
{
    public SqlUnaryExpression(Type type, TypeMapping? typeMapping, ExpressionType operatorType, SqlExpression operand) : base(type, typeMapping)
    {
        OperatorType = operatorType;
        Operand = operand;
    }

    public SqlUnaryExpression(TypeMapping typeMapping, ExpressionType operatorType, SqlExpression operand) : base(typeMapping.ClrType, typeMapping)
    {
        OperatorType = operatorType;
        Operand = operand;
    }

    public ExpressionType OperatorType { get; }
    public SqlExpression Operand { get; }
}