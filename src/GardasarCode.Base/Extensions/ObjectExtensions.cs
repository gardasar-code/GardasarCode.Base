using System.Reflection;

namespace GardasarCode.Base.Extensions;

public static class ObjectExtensions
{
    public static Dictionary<string, TZ?> GetAllStaticProperties<T, TZ>() where T : class
    {
        var fields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fields.Where(w => w.PropertyType == typeof(TZ)).Select(f => new KeyValuePair<string, TZ?>(f.Name, (TZ)f.GetValue(null)!)).ToDictionary(k => k.Key, v => v.Value);
    }
}
