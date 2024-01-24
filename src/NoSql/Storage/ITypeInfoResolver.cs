namespace NoSql.Storage;

public interface ITypeInfoResolver
{
    NoSqlTypeInfo GetTypeInfo(Type type, string? name = null);
}