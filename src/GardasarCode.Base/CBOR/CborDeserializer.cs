using System.Collections;
using System.Formats.Cbor;
using System.Reflection;
using GardasarCode.Base.Extensions;
using GardasarCode.Generator;

namespace GardasarCode.Base.CBOR;

public static class CborDeserializer
{
    public static T? DeserializeFromCbor<T>(CborBytes cbor)
    {
        var reader = new CborReader((byte[])cbor);
        var result = DeserializeObject(reader, typeof(T));
        return (T?)result;
    }

    private static object? DeserializeObject(CborReader reader, Type targetType)
    {
        var cborState = reader.PeekState();

        if (cborState == CborReaderState.Null)
        {
            reader.ReadNull();
            return null;
        }

        var readObjectType = targetType;

        var isTagged = cborState == CborReaderState.Tag;
        var isWrapped = false;

        if (isTagged)
        {
            var pt = reader.PeekTag();

            if (pt >= CborTagTypes.CborUltimo)
            {
                readObjectType = CborTagTypes.UnTag(reader.ReadTag());

                if (reader.PeekState() == CborReaderState.StartMap)
                {
                    reader.ReadStartMap();
                    var selfDescribeCborType = reader.ReadTextString();

                    if (readObjectType is null)
                    {
                        var assemblyType = selfDescribeCborType.Split('|');
                        var assembly = Assembly.Load(assemblyType[1]);
                        readObjectType = assembly.GetType(assemblyType[0]);
                    }

                    isWrapped = true;
                }
            }
        }

        #region Deserialize

        object? result;
        {
            cborState = reader.PeekState();

            if (cborState == CborReaderState.SimpleValue)
            {
                var value = reader.ReadSimpleValue();
                result = value switch
                {
                    CborSimpleValue.Undefined => readObjectType == typeof(string) ? string.Empty : readObjectType == typeof(byte[]) ? Array.Empty<byte>() : Activator.CreateInstance(readObjectType),
                    _ => null
                };
            }
            else if (readObjectType == typeof(string))
            {
                result = reader.ReadTextString();
            }
            else if (readObjectType.IsPrimitiveOrNullablePrimitive())
            {
                result = DeserializeValueType(reader, readObjectType);
            }
            else if (cborState == CborReaderState.StartArray && readObjectType.IsArray)
            {
                result = DeserializeArrayType(reader, readObjectType);
            }
            else if (cborState == CborReaderState.StartArray && readObjectType.IsGenericType && readObjectType.GetGenericTypeDefinition() == typeof(List<>))
            {
                result = DeserializeCollectionType(reader, readObjectType);
            }
            else if (cborState == CborReaderState.StartMap && readObjectType.IsGenericType && readObjectType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                result = DeserializeDictionaryType(reader, readObjectType);
            }
            else
            {

                var properties = readObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(w => w.CanWrite).ToArray();
                if(properties.Length == 0)
                    throw new Exception($"The object of \"{readObjectType.Name}\" cannot be restored because there are no writable fields.");

                result = Activator.CreateInstance(readObjectType);
                var lengthOfProps = reader.ReadStartMap();

                for (var i = 0; i < lengthOfProps; i++)
                {
                    var propertyName = reader.ReadTextString();
                    var property = readObjectType.GetProperty(propertyName);

                    if (property != null && property.CanWrite)
                    {
                        var propertyValue = DeserializeObject(reader, property.PropertyType);
                        property.SetValue(result, propertyValue);
                    }
                    else
                    {
                        reader.SkipValue();
                    }
                }

                reader.ReadEndMap();
            }
        }

        #endregion

        if (isWrapped) reader.ReadEndMap();

        return result;
    }

    private static object? DeserializeDictionaryType(CborReader reader, Type readObjectType)
    {
        var keyType = readObjectType.GetGenericArguments()[0];
        var valueType = readObjectType.GetGenericArguments()[1];

        var dict = (IDictionary)Activator.CreateInstance(readObjectType);
        var length = reader.ReadStartMap() ?? 0;

        for (var i = 0; i < length; i++)
        {
            var key = DeserializeObject(reader, keyType);
            var value = DeserializeObject(reader, valueType);
            dict.Add(key, value);
        }

        reader.ReadEndMap();
        return dict;
    }

    private static object? DeserializeCollectionType(CborReader reader, Type readObjectType)
    {
        var elementType = readObjectType.GetGenericArguments()[0];
        var length = reader.ReadStartArray() ?? 0;

        var list = (IList)Activator.CreateInstance(readObjectType);
        for (var i = 0; i < length; i++) list.Add(DeserializeObject(reader, elementType));

        reader.ReadEndArray();
        return list;
    }

    private static object DeserializeArrayType(CborReader reader, Type readObjectType)
    {
        var arrayType = readObjectType.GetElementType();
        var length = reader.ReadStartArray() ?? 0;

        var array = readObjectType == typeof(byte[]) ? new byte[length] : Array.CreateInstance(arrayType, length);
        for (var i = 0; i < length; i++) array.SetValue(DeserializeObject(reader, arrayType), i);

        reader.ReadEndArray();
        return array;
    }

    private static object? DeserializeValueType(CborReader reader, Type readObjectType)
    {
        return CborTagTypes.DeserializeType(reader, readObjectType);
    }
}
