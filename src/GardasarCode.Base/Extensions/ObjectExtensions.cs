using System.Reflection;

namespace GardasarCode.Base.Extensions;

public static class ObjectExtensions
{
    public static bool IsPrimitiveOrNullablePrimitive(this Type? type)
    {
        // Проверяем, если это Nullable тип
        var underlyingType = Nullable.GetUnderlyingType(type);

        // Если это Nullable, проверяем, что основное значение является примитивом или decimal
        if (underlyingType != null)
        {
            return underlyingType.IsPrimitive || underlyingType == typeof(decimal) || underlyingType == typeof(Guid) || underlyingType == typeof(DateTime) || underlyingType == typeof(DateTimeOffset) || underlyingType == typeof(TimeSpan);
        }
        // Примитивные типы или decimal
        return type.IsPrimitive || type == typeof(decimal) || type == typeof(Guid) || type == typeof(DateTime) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan);
    }
    public static Dictionary<string, TZ?> GetAllStaticProperties<T, TZ>() where T : class
    {
        var fields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fields.Where(w => w.PropertyType == typeof(TZ)).Select(f => new KeyValuePair<string, TZ?>(f.Name, (TZ)f.GetValue(null)!)).ToDictionary(k => k.Key, v => v.Value);
    }
}
