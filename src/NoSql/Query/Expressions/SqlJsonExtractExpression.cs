using NoSql.Storage;

namespace NoSql.Query.Expressions;

public sealed class SqlJsonExtractExpression : SqlExpression
{
    public SqlJsonExtractExpression(Type type, TypeMapping? typeMapping, SqlExpression column, params PathSegment[] path) : base(type, typeMapping)
    {
        Column = column;
        Path = path;
    }

    public SqlJsonExtractExpression(Type type, TypeMapping? typeMapping, SqlExpression column, params string[] members) : base(type, typeMapping)
    {
        Column = column;
        Path = members.Select(x => new PathSegment(x)).ToArray();
    }

    public SqlJsonExtractExpression(TypeMapping typeMapping, SqlExpression column, params PathSegment[] path) : base(typeMapping.ClrType, typeMapping)
    {
        Column = column;
        Path = path;
    }

    public SqlExpression Column { get; }

    public PathSegment[] Path { get; }

    public SqlJsonExtractExpression Extract(TypeMapping typeMapping, params PathSegment[] path)
    {
        PathSegment[] newPath = new PathSegment[path.Length + Path.Length];
        Array.Copy(Path, newPath, Path.Length);
        Array.Copy(path, 0, newPath, Path.Length, path.Length);
        return new SqlJsonExtractExpression(typeMapping, Column, newPath);
    }

    public SqlJsonObjectExpression ToPatchObject(TypeMapping objectTypeMapping, SqlExpression value)
    {
        var result = new SqlJsonObjectExpression(Column.TypeMapping!);
        var obj = result;
        for (int i = 0; i < Path.Length; i++)
        {
            PathSegment segment = Path[i];
            if (segment.PropertyName == null)
            {
                throw new Exception();
            }

            if (i == Path.Length - 1)
            {
                obj.Properties.Add(segment.PropertyName, value);
            }
            else
            {
                var subObj = new SqlJsonObjectExpression(objectTypeMapping);
                obj.Properties.Add(segment.PropertyName, subObj);
                obj = subObj;
            }
        }
        return result;
    }
}
