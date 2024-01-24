using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NoSql.Extensions;

public static class ReflectionExtensions
{
    public static bool IsNullableValueType(this Type type)
        => type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public static bool IsNullableType(this Type type)
        => !type.IsValueType || type.IsNullableValueType();

    public static Type UnwrapNullableType(this Type type) => Nullable.GetUnderlyingType(type) ?? type;

    public static bool IsAnonymousType(this Type type)
        => type.Name.StartsWith("<>", StringComparison.Ordinal)
            && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Length > 0
            && type.Name.Contains("AnonymousType");

    public static Type? GetEnumerableItemType(this Type type)
    {
        if (type.IsArray)
        {
            return type.GetElementType()!;
        }

        return type.GetCompatibleGenericInterface(typeof(IEnumerable<>))?.GenericTypeArguments[0];
    }

    public static bool IsNumeric(this Type type)
    {
        type = type.UnwrapNullableType();

        return type.IsInteger()
            || type == typeof(decimal)
            || type == typeof(float)
            || type == typeof(double);
    }

    public static bool IsInteger(this Type type)
    {
        type = type.UnwrapNullableType();

        return type == typeof(int)
            || type == typeof(long)
            || type == typeof(short)
            || type == typeof(byte)
            || type == typeof(uint)
            || type == typeof(ulong)
            || type == typeof(ushort)
            || type == typeof(sbyte)
            || type == typeof(char);
    }

    public static Type? GetCompatibleGenericInterface(this Type type, Type? interfaceType)
    {
        if (interfaceType is null)
        {
            return null;
        }

        Debug.Assert(interfaceType.IsGenericType);
        Debug.Assert(interfaceType.IsInterface);
        Debug.Assert(interfaceType == interfaceType.GetGenericTypeDefinition());

        Type interfaceToCheck = type;

        if (interfaceToCheck.IsGenericType)
        {
            interfaceToCheck = interfaceToCheck.GetGenericTypeDefinition();
        }

        if (interfaceToCheck == interfaceType)
        {
            return type;
        }

        foreach (Type typeToCheck in type.GetInterfaces())
        {
            if (typeToCheck.IsGenericType)
            {
                Type genericInterfaceToCheck = typeToCheck.GetGenericTypeDefinition();
                if (genericInterfaceToCheck == interfaceType)
                {
                    return typeToCheck;
                }
            }
        }

        return null;
    }
}
