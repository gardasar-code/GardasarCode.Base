using System.Collections;
using System.Formats.Cbor;
using System.Net;
using System.Reflection;
using GardasarCode.Base.Extensions;

namespace GardasarCode.Base.CBOR;

public static class CborSerializer
{
    public static CborBytes SerializeToCbor<T>(T obj)
    {
        var writer = new CborWriter();
        SerializeObject(writer, obj);
        return (CborBytes)writer.Encode();
    }

    private static void SerializeObject<T>(CborWriter writer, T? obj, Type? targetType = null)
    {
        try
        {
            if (obj == null)
            {
                writer.WriteNull();
                return;
            }

            var type = targetType ?? (typeof(T) == typeof(object) ? typeof(object) : obj.GetType());

            if (ValueTypeMethod(writer, obj, type)) return;
            if (ClassTypeMethod(writer, obj, type)) return;
            if (ArrayTypeMethod(writer, obj, type)) return;
            if (CollectionTypeMethod(writer, obj, type)) return;
            if (DictionaryTypeMethod(writer, obj, type)) return;

            writer.WrapObject(type, obj, (cborWriter, _value) =>
            {
                var realObjType = obj.GetType();

                if (realObjType.IsValueType)
                {
                    if (!ValueTypeMethod(cborWriter, _value, realObjType))
                        throw new Exception($"unknown type {realObjType.Name}");
                    return true;
                }

                if (ClassTypeMethod(cborWriter, _value, realObjType)) return true;
                if (ArrayTypeMethod(cborWriter, _value, realObjType)) return true;
                if (CollectionTypeMethod(cborWriter, _value, realObjType)) return true;
                if (DictionaryTypeMethod(cborWriter, _value, realObjType)) return true;

                var properties = realObjType.GetProperties();
                properties = realObjType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var length = properties.Length == 0 ? 1 : properties.Length;


                if (properties.Length == 0)
                {
                    cborWriter.WriteStartMap(length);
                    cborWriter.WriteTextString(realObjType.AssemblyQualifiedName);
                    cborWriter.WriteTag((CborTag)333);
                    cborWriter.WriteTextString("");
                    cborWriter.WriteEndMap();
                }
                else
                {
                    cborWriter.WriteStartMap(1);

                    cborWriter.WriteTextString(realObjType.AssemblyQualifiedName);
                    cborWriter.WriteTag((CborTag)777);

                    cborWriter.WriteStartMap(length);

                    foreach (var prop in properties)
                    {
                        cborWriter.WriteTextString(prop.Name);

                        var indexParams = prop.GetIndexParameters();
                        if (indexParams.Length == 0)
                        {
                            SerializeObject(cborWriter, prop.GetValue(_value), prop.PropertyType);
                        }
                        else
                        {
                            var countProperty = realObjType.GetProperty("Count") ?? realObjType.GetProperty("Length");
                            var count = (int)countProperty.GetValue(_value);

                            var valuesArray = Array.CreateInstance(prop.PropertyType, count);

                            for (var index = 0; index < count; index++) valuesArray.SetValue(prop.GetValue(_value, [index]), index);

                            SerializeObject(cborWriter, valuesArray);
                        }
                    }

                    cborWriter.WriteEndMap();
                    cborWriter.WriteEndMap();
                }

                return true;
            });
        }
        catch (Exception e)
        {
            var message = $"obj: {obj?.ToString() ?? "null"}, objType: {obj?.GetType().Name ?? "unknown"}, targetType: {targetType?.Name ?? "null"}";
            throw new Exception(e.Message + "\n" + message, e);
        }
    }

    private static bool DictionaryTypeMethod<T>(CborWriter writer, T obj, Type type)
    {
        if (type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(Dictionary<,>) && obj is IDictionary dict)
        {
            var elementType = dict.GetType();
            var keyType = elementType.GetGenericArguments()[0];
            var valueType = elementType.GetGenericArguments()[1];

            var length = dict.Count == 0 ? 1 : dict.Count;

            writer.WriteStartMap(length);

            if (dict.Count == 0)
            {
                writer.WriteTag((CborTag)666);
                writer.WriteTextString("");
                writer.WriteTextString("");
                writer.WriteEndMap();
                return true;
            }

            foreach (DictionaryEntry item in dict)
            {
                var keyTargetType = keyType == typeof(object) ? typeof(object) : item.Key?.GetType();
                var valueTargetType = valueType == typeof(object) ? typeof(object) : item.Value?.GetType();

                SerializeObject(writer, item.Key, keyTargetType);
                SerializeObject(writer, item.Value, valueTargetType);
            }

            writer.WriteEndMap();
            return true;
        }

        return false;
    }

    private static bool CollectionTypeMethod<T>(CborWriter writer, T obj, Type type)
    {
        if (type is { IsGenericType: true } && type.GetGenericTypeDefinition() == typeof(List<>) && obj is ICollection collection)
        {
            var elementType = collection.GetType();
            var genericType = elementType.GetGenericArguments()[0];

            writer.WriteStartArray(collection.Count);
            foreach (var item in collection)
            {
                var itemTargetType = genericType == typeof(object) ? typeof(object) : item?.GetType();
                SerializeObject(writer, item, itemTargetType);
            }

            writer.WriteEndArray();
            return true;
        }

        return false;
    }

    private static bool ArrayTypeMethod<T>(CborWriter writer, T obj, Type type)
    {
        if (type is { IsArray: true } && obj is Array array)
        {
            //var array = obj as Array;

            var arrayType = array.GetType();
            var length = array.Length == 0 ? 1 : array.Length;

            writer.WriteStartArray(length);

            if (array.Length == 0)
            {
                writer.WriteTag((CborTag)666);
                writer.WriteTextString("");
                writer.WriteEndArray();
                return true;
            }

            foreach (var item in array)
            {
                var itemTargetType = arrayType == typeof(object[]) ? typeof(object) : item?.GetType();
                SerializeObject(writer, item, itemTargetType);
            }

            writer.WriteEndArray();
            return true;
        }

        return false;
    }

    private static bool ClassTypeMethod<T>(CborWriter writer, T? obj, Type? type)
    {
        {
            if (type == typeof(IPAddress) && obj is IPAddress value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteTextString(_value.ToString());
                    return true;
                });
        }

        {
            if (type == typeof(string) && obj is string value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteTextString(_value.ToString());
                    return true;
                });
        }

        return false;
    }

    private static bool WrapObject<T>(this CborWriter writer, Type targetType, T? obj, Func<CborWriter, T?, bool> func)
    {
        bool result;
        if (targetType != typeof(object))
        {
            result = func(writer, obj);
        }
        else if (targetType == typeof(object) && targetType == obj.GetType())
        {
            result = func(writer, obj);
        }
        else
        {
            writer.WriteStartMap(1);
            writer.WriteTextString(obj.GetType().AssemblyQualifiedName);
            writer.WriteTag((CborTag)999);

            result = func(writer, obj);
            writer.WriteEndMap();
        }

        return result;
    }

    private static bool ValueTypeMethod<T>(CborWriter writer, T? obj, Type? type)
    {
        if (type is not { IsValueType: true }) return false;

        {
            if (type == typeof(Guid) && obj is Guid value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteTextString(_value.ToString());
                    return true;
                });
        }

        {
            if (type == typeof(Byte) && obj is Byte value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteInt32(value);
                    return true;
                });
        }

        {
            if (type == typeof(Int16) && obj is Int16 value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteInt32(_value);
                    return true;
                });
        }

        {
            if (type == typeof(UInt16) && obj is UInt16 value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteUInt32(_value);
                    return true;
                });
        }

        {
            if (type == typeof(Int32) && obj is Int32 value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteInt32(_value);
                    return true;
                });
        }

        {
            if (type == typeof(UInt32) && obj is UInt32 value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteUInt32(_value);
                    return true;
                });
        }

        {
            if (type == typeof(Int64) && obj is Int64 value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteInt64(_value);
                    return true;
                });
        }

        {
            if (type == typeof(UInt64) && obj is UInt64 value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteUInt64(_value);
                    return true;
                });
        }

        {
            if (type == typeof(decimal) && obj is decimal value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteDecimal(_value);
                    return true;
                });
        }

        {
            if (type == typeof(double) && obj is double value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteDouble(_value);
                    return true;
                });
        }

        {
            if (type == typeof(Single) && obj is Single value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteSingle(_value);
                    return true;
                });
        }

        {
            if (type == typeof(Boolean) && obj is Boolean value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteBoolean(_value);
                    return true;
                });
        }

        {
            if (type == typeof(TimeSpan) && obj is TimeSpan value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteDouble(_value.TotalMicroseconds);
                    return true;
                });
        }

        {
            if (type == typeof(DateTime) && obj is DateTime value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteUnixTimeSeconds(_value.ToUnixTicks());
                    return true;
                });
        }

        {
            if (type == typeof(DateTimeOffset) && obj is DateTimeOffset value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteDateTimeOffset(_value);
                    return true;
                });
        }

        {
            if (type == typeof(char) && obj is char value)
                return writer.WrapObject(type, value, (cborWriter, _value) =>
                {
                    cborWriter.WriteTextString(_value.ToString());
                    return true;
                });
        }

        return false;
    }
}
