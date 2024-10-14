using System.Collections;
using System.Formats.Cbor;

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

        cborState = reader.PeekState();
        if (cborState == CborReaderState.Tag)
        {
            var tag = reader.ReadTag();
            switch (tag)
            {
                case (CborTag)333:
                    reader.ReadTextString();
                    return Activator.CreateInstance(targetType);
            }
        }

        object? result = null;

        if (cborState != CborReaderState.StartMap)
        {
            if (targetType == typeof(Int16) || targetType == typeof(Int16))
            {
                result = (Int16)reader.ReadInt32();
            }
            else if (targetType == typeof(UInt16) || targetType == typeof(UInt16?))
            {
                result = (UInt16)reader.ReadUInt32();
            }
            else if (targetType == typeof(Int32) || targetType == typeof(Int32?))
            {
                result = reader.ReadInt32();
            }
            else if (targetType == typeof(UInt32) || targetType == typeof(UInt32?))
            {
                result = reader.ReadUInt32();
            }
            else if (targetType == typeof(Int64) || targetType == typeof(Int64?))
            {
                result = reader.ReadInt64();
            }
            else if (targetType == typeof(UInt64) || targetType == typeof(UInt64?))
            {
                result = reader.ReadUInt64();
            }
            else if (targetType == typeof(Decimal) || targetType == typeof(Decimal?))
            {
                result = reader.ReadDecimal();
            }
            else if (targetType == typeof(Double) || targetType == typeof(Double?))
            {
                result = reader.ReadDouble();
            }
            else if (targetType == typeof(Single) || targetType == typeof(Single?))
            {
                result = reader.ReadSingle();
            }
            else if (targetType == typeof(Byte) || targetType == typeof(Byte?))
            {
                result = (byte?)reader.ReadInt32();
            }
            else if (targetType == typeof(Boolean) || targetType == typeof(Boolean?))
            {
                result = reader.ReadBoolean();
            }
            else if (targetType == typeof(TimeSpan) || targetType == typeof(TimeSpan?))
            {
                result = TimeSpan.FromMicroseconds(reader.ReadDouble());
            }
            else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
            {
                result = reader.ReadUnixTimeSeconds();
            }
            else if (targetType == typeof(DateTimeOffset) || targetType == typeof(DateTimeOffset?))
            {
                result = reader.ReadDateTimeOffset();
            }
            else if (targetType == typeof(Char) || targetType == typeof(Char?))
            {
                result = reader.ReadTextString().ToCharArray()[0];
            }
            else if (targetType == typeof(String))
            {
                result = reader.ReadTextString();
            }
            else if (targetType == typeof(Guid))
            {
                result = new Guid(reader.ReadTextString());
            }
            else if (targetType.IsArray)
            {
                var elementType = targetType.GetElementType();
                var length = reader.ReadStartArray() ?? 0;

                var arrayState = reader.PeekState();
                if (arrayState == CborReaderState.Tag)
                {
                    var tag = reader.ReadTag();
                    return Array.CreateInstance(elementType, 0);
                }

                var array = targetType == typeof(Byte[]) ? new Byte[length] : Array.CreateInstance(elementType, length);
                for (var i = 0; i < length; i++) array.SetValue(DeserializeObject(reader, elementType), i);

                reader.ReadEndArray();
                result = array;
            }
            else if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = targetType.GetGenericArguments()[0];
                var length = reader.ReadStartArray() ?? 0;

                var list = (IList)Activator.CreateInstance(targetType);
                for (var i = 0; i < length; i++) list.Add(DeserializeObject(reader, elementType));

                reader.ReadEndArray();
                result = list;
            }
        }
        else if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            var keyType = targetType.GetGenericArguments()[0];
            var valueType = targetType.GetGenericArguments()[1];

            var dict = (IDictionary)Activator.CreateInstance(targetType);
            var length = reader.ReadStartMap();

            var dictState = reader.PeekState();
            if (dictState == CborReaderState.Tag)
            {
                var tag = reader.ReadTag();
                reader.ReadTextString();
                reader.ReadTextString();
                return (IDictionary)Activator.CreateInstance(targetType);
            }

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
            if (cborState == CborReaderState.Tag)
            {
                throw new Exception($"unknown type {targetType.Name}");
            }

            reader.ReadStartMap();
            var objectType = reader.ReadTextString();
            targetType = Type.GetType(objectType);

            object? instance = null; //targetType == typeof(string) ? string.Empty : Activator.CreateInstance(targetType);

            if (targetType.IsValueType)
            {
                instance = Activator.CreateInstance(targetType);
                var value = DeserializeObject(reader, targetType);
                instance = value;
            }
            else
            {
                if (targetType == typeof(string))
                {
                    instance = string.Empty;
                    var value = DeserializeObject(reader, targetType);
                    instance = value;
                }
                else if (targetType == typeof(object))
                {
                    var tag = reader.ReadTag();
                    reader.ReadTextString();
                    var value = new object();
                    instance = value;
                }
                else
                {
                    var objectState = reader.PeekState();
                    var objectStateTag = reader.ReadTag();

                    if (objectStateTag == (CborTag)999)
                    {
                        var value = DeserializeObject(reader, targetType);
                        instance = value;
                    }
                    else
                    {
                        instance = Activator.CreateInstance(targetType);

                        objectState = reader.PeekState();
                        if (objectState == CborReaderState.StartMap)
                        {
                            var length = reader.ReadStartMap();

                            for (var i = 0; i < length; i++)
                            {
                                var propertyName = reader.ReadTextString();

                                objectState = reader.PeekState();
                                if (objectState == CborReaderState.Tag)
                                {
                                    var tag = reader.ReadTag();
                                    reader.ReadTextString();
                                }
                                else
                                {
                                    var property = targetType.GetProperty(propertyName);
                                    if (property != null && property.CanWrite)
                                    {
                                        var propertyValue = DeserializeObject(reader, property.PropertyType);
                                        property.SetValue(instance, propertyValue);
                                    }
                                    else
                                    {
                                        reader.SkipValue();
                                    }
                                }
                            }


                            reader.ReadEndMap();
                        }
                        else
                        {
                            reader.ReadTextString();
                        }
                    }
                }
            }

            reader.ReadEndMap();

            result = instance;
        }

        return result;
    }
}
