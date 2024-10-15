using System.Formats.Cbor;

namespace GardasarCode.Base.CBOR;

public static class Cbor
{
    private const string EmptyByte = "F6";
    private static readonly ulong ii = 1;

    public static readonly CborTag CborNew = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborInt16 = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborInt16Nullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborInt16Array = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborInt32 = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborInt32Nullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborInt32Array = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborInt64 = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborInt64Nullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborInt64Array = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborUInt32 = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborUInt32Nullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborUInt32Array = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborUInt64 = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborUInt64Nullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborUInt64Array = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborSingle = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborSingleNullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborSingleArray = CborTag.SelfDescribeCbor - ii++;

    private static readonly CborTag CborDouble = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborDoubleNullable = CborTag.SelfDescribeCbor - ii++;
    private static readonly CborTag CborDoubleArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborBoolean = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborBooleanNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborBooleanArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborString = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborStringNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborStringArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborByte = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborByteNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborByteArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborChar = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborCharNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborCharArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborSByte = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborSByteNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborSByteArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborDecimal = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborDecimalNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborDecimalArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborDateTime = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborDateTimeNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborDateTimeArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborTimeSpan = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborTimeSpanNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborTimeSpanArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborGuid = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborGuidNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborGuidArray = CborTag.SelfDescribeCbor - ii++;

    public static readonly CborTag CborDateTimeOffset = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborDateTimeOffsetNullable = CborTag.SelfDescribeCbor - ii++;
    public static readonly CborTag CborDateTimeOffsetArray = CborTag.SelfDescribeCbor - ii++;

    public static (CborTag, string?) Tagging(Type type)
    {
        return type switch
        {
            not null when type == typeof(short) => (CborInt16, EmptyByte),
            not null when type == typeof(short?) => (CborInt16Nullable, EmptyByte),
            not null when type == typeof(short[]) => (CborInt16Array, EmptyByte),

            not null when type == typeof(int) => (CborInt32, EmptyByte),
            not null when type == typeof(int?) => (CborInt32Nullable, EmptyByte),
            not null when type == typeof(int[]) => (CborInt32Array, EmptyByte),

            not null when type == typeof(long) => (CborInt64, EmptyByte),
            not null when type == typeof(long?) => (CborInt64Nullable, EmptyByte),
            not null when type == typeof(long[]) => (CborInt64Array, EmptyByte),

            not null when type == typeof(uint) => (CborUInt32, EmptyByte),
            not null when type == typeof(uint?) => (CborUInt32Nullable, EmptyByte),
            not null when type == typeof(uint[]) => (CborUInt32Array, EmptyByte),

            not null when type == typeof(ulong) => (CborUInt64, EmptyByte),
            not null when type == typeof(ulong?) => (CborUInt64Nullable, EmptyByte),
            not null when type == typeof(ulong[]) => (CborUInt64Array, EmptyByte),

            not null when type == typeof(float) => (CborSingle, EmptyByte),
            not null when type == typeof(float?) => (CborSingleNullable, EmptyByte),
            not null when type == typeof(float[]) => (CborSingleArray, EmptyByte),

            not null when type == typeof(double) => (CborDouble, EmptyByte),
            not null when type == typeof(double?) => (CborDoubleNullable, EmptyByte),
            not null when type == typeof(double[]) => (CborDoubleArray, EmptyByte),

            not null when type == typeof(bool) => (CborBoolean, EmptyByte),
            not null when type == typeof(bool?) => (CborBooleanNullable, EmptyByte),
            not null when type == typeof(bool[]) => (CborBooleanArray, EmptyByte),

            not null when type == typeof(string) => (CborString, EmptyByte),
            not null when type == typeof(string[]) => (CborStringNullable, EmptyByte),
            not null when type == typeof(string[]) => (CborStringArray, EmptyByte),

            not null when type == typeof(byte) => (CborByte, EmptyByte),
            not null when type == typeof(byte?) => (CborByteNullable, EmptyByte),
            not null when type == typeof(byte[]) => (CborByteArray, EmptyByte),

            not null when type == typeof(char) => (CborChar, EmptyByte),
            not null when type == typeof(char?) => (CborCharNullable, EmptyByte),
            not null when type == typeof(char[]) => (CborCharArray, EmptyByte),

            not null when type == typeof(sbyte) => (CborSByte, EmptyByte),
            not null when type == typeof(sbyte?) => (CborSByteNullable, EmptyByte),
            not null when type == typeof(sbyte[]) => (CborSByteArray, EmptyByte),

            not null when type == typeof(decimal) => (CborDecimal, EmptyByte),
            not null when type == typeof(decimal?) => (CborDecimalNullable, EmptyByte),
            not null when type == typeof(decimal[]) => (CborDecimalArray, EmptyByte),

            not null when type == typeof(DateTime) => (CborDateTime, EmptyByte),
            not null when type == typeof(DateTime?) => (CborDateTimeNullable, EmptyByte),
            not null when type == typeof(DateTime[]) => (CborDateTimeArray, EmptyByte),

            not null when type == typeof(TimeSpan) => (CborTimeSpan, EmptyByte),
            not null when type == typeof(TimeSpan?) => (CborTimeSpanNullable, EmptyByte),
            not null when type == typeof(TimeSpan[]) => (CborTimeSpanArray, EmptyByte),

            not null when type == typeof(Guid) => (CborGuid, EmptyByte),
            not null when type == typeof(Guid?) => (CborGuidNullable, EmptyByte),
            not null when type == typeof(Guid[]) => (CborGuidArray, EmptyByte),

            not null when type == typeof(DateTimeOffset) => (CborDateTimeOffset, EmptyByte),
            not null when type == typeof(DateTimeOffset?) => (CborDateTimeOffsetNullable, EmptyByte),
            not null when type == typeof(DateTimeOffset[]) => (CborDateTimeOffsetArray, EmptyByte),


            _ => (CborTag.SelfDescribeCbor, type.AssemblyQualifiedName)
        };
    }

    public static Type? GetTypeFromCborTag(CborTag tag)
    {
        return tag switch
        {
            CborTag _ when tag == CborInt16 => typeof(short),
            CborTag _ when tag == CborInt16Nullable => typeof(short?),
            CborTag _ when tag == CborInt16Array => typeof(short[]),

            CborTag _ when tag == CborInt32 => typeof(int),
            CborTag _ when tag == CborInt32Nullable => typeof(int?),
            CborTag _ when tag == CborInt32Array => typeof(int[]),

            CborTag _ when tag == CborInt64 => typeof(long),
            CborTag _ when tag == CborInt64Nullable => typeof(long?),
            CborTag _ when tag == CborInt64Array => typeof(long[]),

            CborTag _ when tag == CborUInt32 => typeof(uint),
            CborTag _ when tag == CborUInt32Nullable => typeof(uint?),
            CborTag _ when tag == CborUInt32Array => typeof(uint[]),

            CborTag _ when tag == CborUInt64 => typeof(ulong),
            CborTag _ when tag == CborUInt64Nullable => typeof(ulong?),
            CborTag _ when tag == CborUInt64Array => typeof(ulong[]),

            CborTag _ when tag == CborSingle => typeof(float),
            CborTag _ when tag == CborSingleNullable => typeof(float?),
            CborTag _ when tag == CborSingleArray => typeof(float[]),

            CborTag _ when tag == CborDouble => typeof(double),
            CborTag _ when tag == CborDoubleNullable => typeof(double?),
            CborTag _ when tag == CborDoubleArray => typeof(double[]),

            CborTag _ when tag == CborBoolean => typeof(bool),
            CborTag _ when tag == CborBooleanNullable => typeof(bool?),
            CborTag _ when tag == CborBooleanArray => typeof(bool[]),

            CborTag _ when tag == CborString => typeof(string),
            CborTag _ when tag == CborStringNullable => typeof(string),
            CborTag _ when tag == CborStringArray => typeof(string[]),

            CborTag _ when tag == CborByte => typeof(byte),
            CborTag _ when tag == CborByteNullable => typeof(byte?),
            CborTag _ when tag == CborByteArray => typeof(byte[]),

            CborTag _ when tag == CborChar => typeof(char),
            CborTag _ when tag == CborCharNullable => typeof(char?),
            CborTag _ when tag == CborCharArray => typeof(char[]),

            CborTag _ when tag == CborSByte => typeof(sbyte),
            CborTag _ when tag == CborSByteNullable => typeof(sbyte?),
            CborTag _ when tag == CborSByteArray => typeof(sbyte[]),

            CborTag _ when tag == CborDecimal => typeof(decimal),
            CborTag _ when tag == CborDecimalNullable => typeof(decimal?),
            CborTag _ when tag == CborDecimalArray => typeof(decimal[]),

            CborTag _ when tag == CborDateTime => typeof(DateTime),
            CborTag _ when tag == CborDateTimeNullable => typeof(DateTime?),
            CborTag _ when tag == CborDateTimeArray => typeof(DateTime[]),

            CborTag _ when tag == CborTimeSpan => typeof(TimeSpan),
            CborTag _ when tag == CborTimeSpanNullable => typeof(TimeSpan?),
            CborTag _ when tag == CborTimeSpanArray => typeof(TimeSpan[]),

            CborTag _ when tag == CborGuid => typeof(Guid),
            CborTag _ when tag == CborGuidNullable => typeof(Guid?),
            CborTag _ when tag == CborGuidArray => typeof(Guid[]),

            CborTag _ when tag == CborDateTimeOffset => typeof(DateTimeOffset),
            CborTag _ when tag == CborDateTimeOffsetNullable => typeof(DateTimeOffset?),
            CborTag _ when tag == CborDateTimeOffsetArray => typeof(DateTimeOffset[]),

            _ => null
        };
    }
}
