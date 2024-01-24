using NoSql.Extensions;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace NoSql.Storage;

public class TypeInfoReflectionResolver(ISqlTypeMappingSource typeMappingSource) : ITypeInfoResolver
{
    private readonly ConcurrentDictionary<string, NoSqlTypeInfo> _types = new();

    public NoSqlTypeInfo GetTypeInfo(Type type, string? name = null)
    {
        if (name != null)
        {
            return _types.GetOrAdd(name, n => CreateTypeInfo(type, n));
        }
        else
        {
            name = type.GetCustomAttribute<TableAttribute>()?.Name;
            if (name == null)
            {
                return _types.GetOrAdd(type.FullName!, n => CreateTypeInfo(type, type.Name));
            }
            else
            {
                return _types.GetOrAdd(name, n => CreateTypeInfo(type, n));
            }
        }
    }

    private NoSqlTypeInfo CreateTypeInfo(Type type, string name)
    {
        var typeMapping = typeMappingSource.FindMapping(type);
        if (!typeMapping.IsJsonType)
        {
            return new NoSqlTypeInfo(name, type, typeMapping);
        }

        if (type.IsArray)
        {
            return new NoSqlTypeInfo(name, type, typeMapping);
        }

        if (type.GetCompatibleGenericInterface(typeof(IEnumerable<>)) != null)
        {
            return new NoSqlTypeInfo(name, type, typeMapping);
        }

        List<NoSqlFieldInfo> columns = [];
        List<NoSqlFieldInfo> key = [];

        foreach (var property in type.GetProperties())
        {
            var unmapped = property.GetCustomAttribute<NotMappedAttribute>() != null;
            var fieldTypeMapping = typeMappingSource.FindMapping(property.PropertyType);
            var autoIncrement = property.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity;
            var nullable = property.GetCustomAttribute<RequiredAttribute>() == null && property.PropertyType.IsNullableType();
            var fieldName = property.GetCustomAttribute<ColumnAttribute>()?.Name ?? property.Name;
            var field = new NoSqlFieldInfo(
                                          fieldName,
                                          property.Name,
                                          fieldTypeMapping,
                                          unmapped,
                                          nullable,
                                          autoIncrement,
                                          property.CanRead ? property.GetValue : null,
                                          property.CanWrite ? property.SetValue : null);

            columns.Add(field);

            if (property.GetCustomAttribute<KeyAttribute>() != null)
            {
                key.Add(field);
            }
        }

        List<NoSqlIndexInfo> indexes = [];
        NoSqlPrimaryKeyInfo? primaryKey = null;

        primaryKey = CreatePrimaryKey(name, key);

        var indexAttributes = type.GetCustomAttributes<IndexAttribute>();
        foreach (var indexAttribute in indexAttributes)
        {
            var indexColumn = columns.Where(x => indexAttribute.Columns.Contains(x.Name)).ToList();
            if (indexColumn.Count > 0)
                indexes.Add(CreateIndex(name, indexColumn, indexAttribute.IsDescending));
        }

        bool anonymousType = type.IsAnonymousType();
        Func<object?[]?, object> createInstance = anonymousType ?
            type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0].Invoke :
            type.GetConstructor(Array.Empty<Type>())!.Invoke;

        return new(name: name,
                   clrType: type,
                   createInstance: createInstance,
                   requireConstructorParameters: anonymousType,
                   typeMapping: typeMapping,
                   fields: [.. columns],
                   primaryKey: primaryKey,
                   indexes: [.. indexes]);
    }

    private static NoSqlPrimaryKeyInfo? CreatePrimaryKey(string tableName, List<NoSqlFieldInfo> fields)
    {
        if (fields.Count > 0)
        {
            return new NoSqlPrimaryKeyInfo($"PK_{tableName}", [.. fields]);
        }

        return null;
    }

    private static NoSqlIndexInfo CreateIndex(string tableName, List<NoSqlFieldInfo> fields, bool[] descending)
    {
        return new NoSqlIndexInfo($"IX_{tableName}_{string.Join("_", fields.Select(x => x.Name))}", [.. fields], descending);
    }
}