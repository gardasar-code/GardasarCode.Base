namespace GardasarCode.Base.CBOR;

public class CborBytes
{
    private byte[]? _cborData = Array.Empty<byte>();

    public static implicit operator byte[](CborBytes cborString)
    {
        return cborString._cborData ?? [];
    }

    public static explicit operator CborBytes(byte[] cborData)
    {
        return new CborBytes { _cborData = cborData };
    }
}
