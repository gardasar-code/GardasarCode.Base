using System.Collections;
using System.Formats.Cbor;
using System.Reflection;
using GardasarCode.Base.Extensions;

namespace GardasarCode.Base.CBOR;

public static class CborSerializer
{
    public static CborBytes SerializeToCbor<T>(T? obj)
    {
        var writer = new CborWriter();

        var wrapType = typeof(T);
        var objectType = obj?.GetType() ?? wrapType;

        var _ = SerializeAsObject(writer, obj, wrapType, objectType);
        return (CborBytes)writer.Encode();
    }

    private static bool SerializeAsObject(CborWriter writer, object? obj, Type wrapType, Type saveObjectType)
    {
        if (obj == null)
        {
            writer.WriteNull();
            return true;
        }

        var isWrapping = wrapType == typeof(object) && saveObjectType != typeof(object);

        if (isWrapping)
        {
            var (tag, name) = Cbor.Tagging(saveObjectType);
            writer.WriteTag(tag);

            writer.WriteStartMap(1); // map

            writer.WriteTextString(name); // key
        }

        var properties = saveObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var isSerialized = false;

        // value
        if (properties.Length == 0 && saveObjectType.IsValueType)
        {
            isSerialized = SerializeValueType(writer, obj);
        }
        else if (saveObjectType == typeof(string))
        {
            writer.WriteTextString(obj.ToString());
            isSerialized = true;
        }
        else if (saveObjectType is { IsArray: true })
        {
            isSerialized = SerializeArrayType(writer, obj);
        }
        else if (saveObjectType is { IsGenericType: true } && saveObjectType.GetGenericTypeDefinition() == typeof(List<>))
        {
            isSerialized = SerializeCollectionType(writer, obj);
        }
        else if (saveObjectType is { IsGenericType: true } && saveObjectType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            isSerialized = SerializeDictionaryType(writer, obj);
        }
        else if (!saveObjectType.IsArray && properties.Length > 0)
        {
            writer.WriteStartMap(properties.Length); // map as value

            foreach (var prop in properties)
            {
                writer.WriteTextString(prop.Name); // key

                var propType = prop.PropertyType;
                var propValue = prop.GetValue(obj);

                // value
                isSerialized = SerializeAsObject(writer, propValue, propType, propValue?.GetType() ?? propType);
                if (!isSerialized)
                    throw new Exception($"unknown type {propType.Name}");
            }

            writer.WriteEndMap(); // map end
        }
        else
        {
            writer.WriteSimpleValue(CborSimpleValue.Undefined);
            isSerialized = true;
        }

        if (isWrapping) writer.WriteEndMap(); // map end

        return isSerialized;
    }

    private static bool SerializeValueType<T>(CborWriter writer, T? obj)
    {
        var saveObjectType = obj.GetType();
        if (saveObjectType is not { IsValueType: true }) return false;

        if (saveObjectType == typeof(short) && obj is short _short)
        {
            writer.WriteInt32(_short);
            return true;
        }

        if (saveObjectType == typeof(ushort) && obj is ushort _ushort)
        {
            writer.WriteUInt32(_ushort);
            return true;
        }

        if (saveObjectType == typeof(int) && obj is int _int32)
        {
            writer.WriteInt32(_int32);
            return true;
        }

        if (saveObjectType == typeof(long) && obj is long _int64)
        {
            writer.WriteInt64(_int64);
            return true;
        }

        if (saveObjectType == typeof(uint) && obj is uint _uint32)
        {
            writer.WriteUInt32(_uint32);
            return true;
        }

        if (saveObjectType == typeof(ulong) && obj is ulong _uint64)
        {
            writer.WriteUInt64(_uint64);
            return true;
        }

        if (saveObjectType == typeof(float) && obj is float _single)
        {
            writer.WriteSingle(_single);
            return true;
        }

        if (saveObjectType == typeof(double) && obj is double _double)
        {
            writer.WriteDouble(_double);
            return true;
        }

        if (saveObjectType == typeof(decimal) && obj is decimal _decimal)
        {
            writer.WriteDecimal(_decimal);
            return true;
        }

        if (saveObjectType == typeof(char) && obj is char _char)
        {
            writer.WriteTextString(_char.ToString());
            return true;
        }

        if (saveObjectType == typeof(byte) && obj is byte _byte)
        {
            writer.WriteInt32(_byte);
            return true;
        }

        if (saveObjectType == typeof(sbyte) && obj is sbyte _sbyte)
        {
            writer.WriteInt32(_sbyte);
            return true;
        }

        if (saveObjectType == typeof(TimeSpan) && obj is TimeSpan _timeSpan)
        {
            writer.WriteDouble(_timeSpan.TotalMicroseconds);
            return true;
        }

        if (saveObjectType == typeof(bool) && obj is bool _bool)
        {
            writer.WriteBoolean(_bool);
            return true;
        }

        if (saveObjectType == typeof(Guid) && obj is Guid _guid)
        {
            writer.WriteTextString(_guid.ToString());
            return true;
        }

        if (saveObjectType == typeof(DateTime) && obj is DateTime _dateTime)
        {
            writer.WriteUnixTimeSeconds(_dateTime.ToUnixTicks());
            return true;
        }

        if (saveObjectType == typeof(DateTimeOffset) && obj is DateTimeOffset _dateTimeOffset)
        {
            writer.WriteDateTimeOffset(_dateTimeOffset);
            return true;
        }

        return false;
    }

    private static bool SerializeArrayType(CborWriter writer, object? obj)
    {
        var saveObjectType = obj.GetType();
        if (saveObjectType is { IsArray: true } && obj is Array array)
        {
            var arrayType = array.GetType();

            writer.WriteStartArray(array.Length);

            foreach (var item in array)
            {
                var itemTargetType = arrayType == typeof(object[]) ? typeof(object) : item?.GetType();
                SerializeAsObject(writer, item, itemTargetType, item?.GetType());
            }

            writer.WriteEndArray();
            return true;
        }

        return false;
    }

    private static bool SerializeCollectionType(CborWriter writer, object? obj)
    {
        var saveObjectType = obj.GetType();
        if (saveObjectType is { IsGenericType: true } && saveObjectType.GetGenericTypeDefinition() == typeof(List<>) && obj is ICollection collection)
        {
            var elementType = collection.GetType();
            var genericType = elementType.GetGenericArguments()[0];

            writer.WriteStartArray(collection.Count);
            foreach (var item in collection) SerializeAsObject(writer, item, genericType, item?.GetType() ?? typeof(object));

            writer.WriteEndArray();
            return true;
        }

        return false;
    }

    private static bool SerializeDictionaryType<T>(CborWriter writer, T obj)
    {
        var saveObjectType = obj.GetType();
        if (saveObjectType is { IsGenericType: true } && saveObjectType.GetGenericTypeDefinition() == typeof(Dictionary<,>) && obj is IDictionary dict)
        {
            var elementType = dict.GetType();
            var keyType = elementType.GetGenericArguments()[0];
            var valueType = elementType.GetGenericArguments()[1];

            writer.WriteStartMap(dict.Count);

            foreach (DictionaryEntry item in dict)
            {
                var keyTargetType = keyType == typeof(object) ? typeof(object) : item.Key?.GetType();
                var valueTargetType = valueType == typeof(object) ? typeof(object) : item.Value?.GetType();

                SerializeAsObject(writer, item.Key, keyType, item.Key.GetType());
                SerializeAsObject(writer, item.Value, valueType, item.Value?.GetType() ?? typeof(object));
            }

            writer.WriteEndMap();
            return true;
        }

        return false;
    }
}
