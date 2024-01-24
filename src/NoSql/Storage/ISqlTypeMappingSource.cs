namespace NoSql.Storage;

public interface ISqlTypeMappingSource
{
    TypeMapping FindMapping(Type type, bool jsonType = false);
}
