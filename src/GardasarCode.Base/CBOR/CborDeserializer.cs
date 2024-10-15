using System.Collections;
using System.Formats.Cbor;
using System.Reflection;

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
            readObjectType = Cbor.GetTypeFromCborTag(reader.ReadTag());

            if (reader.PeekState() == CborReaderState.StartMap)
            {
                reader.ReadStartMap();
                var selfDescribeCborType = reader.ReadTextString();

                if (readObjectType is null)
                {
                    string[] assemblyType = selfDescribeCborType.Split('|');
                    Assembly assembly = Assembly.Load(assemblyType[1]);
                    readObjectType = assembly.GetType(assemblyType[0]);
                }

                isWrapped = true;
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
            else if (cborState == CborReaderState.Tag && reader.ReadTag() == Cbor.CborNew)
            {
                reader.ReadNull();
                result = readObjectType == typeof(string) ? string.Empty : readObjectType == typeof(byte[]) ? Array.Empty<byte>() : Activator.CreateInstance(readObjectType);
            }
            else if (readObjectType == typeof(string))
            {
                result = reader.ReadTextString();
            }
            else if (readObjectType.IsValueType)
            {
                result = DeserializeValueType(reader, readObjectType);
            }
            else if (cborState == CborReaderState.StartArray && readObjectType.IsArray)
            {
                var elementType = readObjectType.GetElementType();
                var length = reader.ReadStartArray() ?? 0;

                var array = readObjectType == typeof(byte[]) ? new byte[length] : Array.CreateInstance(elementType, length);
                for (var i = 0; i < length; i++) array.SetValue(DeserializeObject(reader, elementType), i);

                reader.ReadEndArray();
                result = array;
            }
            else if (cborState == CborReaderState.StartArray && readObjectType.IsGenericType && readObjectType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = readObjectType.GetGenericArguments()[0];
                var length = reader.ReadStartArray() ?? 0;

                var list = (IList)Activator.CreateInstance(readObjectType);
                for (var i = 0; i < length; i++) list.Add(DeserializeObject(reader, elementType));

                reader.ReadEndArray();
                result = list;
            }
            else if (cborState == CborReaderState.StartMap && readObjectType.IsGenericType && readObjectType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
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
                result = dict;
            }
            else
            {
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

    private static object? DeserializeValueType(CborReader reader, Type readObjectType)
    {
        if (readObjectType == typeof(short) || readObjectType == typeof(short))
            return (short)reader.ReadInt32();
        if (readObjectType == typeof(ushort) || readObjectType == typeof(ushort?))
            return (ushort)reader.ReadUInt32();
        if (readObjectType == typeof(int) || readObjectType == typeof(int?))
            return reader.ReadInt32();
        if (readObjectType == typeof(uint) || readObjectType == typeof(uint?))
            return reader.ReadUInt32();
        if (readObjectType == typeof(long) || readObjectType == typeof(long?))
            return reader.ReadInt64();
        if (readObjectType == typeof(ulong) || readObjectType == typeof(ulong?))
            return reader.ReadUInt64();
        if (readObjectType == typeof(decimal) || readObjectType == typeof(decimal?))
            return reader.ReadDecimal();
        if (readObjectType == typeof(double) || readObjectType == typeof(double?))
            return reader.ReadDouble();
        if (readObjectType == typeof(float) || readObjectType == typeof(float?))
            return reader.ReadSingle();
        if (readObjectType == typeof(byte) || readObjectType == typeof(byte?))
            return (byte?)reader.ReadInt32();
        if (readObjectType == typeof(bool) || readObjectType == typeof(bool?))
            return reader.ReadBoolean();
        if (readObjectType == typeof(TimeSpan) || readObjectType == typeof(TimeSpan?))
            return TimeSpan.FromMicroseconds(reader.ReadDouble());
        if (readObjectType == typeof(DateTime) || readObjectType == typeof(DateTime?))
            return reader.ReadUnixTimeSeconds();
        if (readObjectType == typeof(DateTimeOffset) || readObjectType == typeof(DateTimeOffset?))
            return reader.ReadDateTimeOffset();
        if (readObjectType == typeof(char) || readObjectType == typeof(char?))
            return reader.ReadTextString().ToCharArray()[0];
        if (readObjectType == typeof(Guid)) return new Guid(reader.ReadTextString());

        return null;
    }
}
