using NoSql.Extensions;
using NoSql.Storage.Mappings;
using System.Diagnostics.CodeAnalysis;

namespace NoSql.Storage;

public abstract class TypeMappingSource : ISqlTypeMappingSource
{
    public virtual TypeMapping FindMapping(Type type, bool jsonType = false)
    {
        var underingType = type.UnwrapNullableType();

        if (underingType.IsEnum)
        {
            underingType = underingType.GetEnumUnderlyingType();
        }

        if (TryGetMapping(underingType, out var mapping))
        {
            return mapping;
        }

        return GetJsonTypeMapping(underingType);
    }

    public abstract JsonTypeMapping GetJsonTypeMapping(Type type);

    protected abstract bool TryGetMapping(Type type, [NotNullWhen(true)] out TypeMapping? mapping);
}