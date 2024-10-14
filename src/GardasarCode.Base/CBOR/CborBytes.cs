namespace GardasarCode.Base.CBOR;

public class CborBytes
{
    private byte[]? cborData = Array.Empty<byte>();

    public static implicit operator byte[](CborBytes cborString)
    {
        return cborString.cborData ?? [];
    }

    public static explicit operator CborBytes(byte[] cborData)
    {
        return new CborBytes { cborData = cborData };
    }
}
