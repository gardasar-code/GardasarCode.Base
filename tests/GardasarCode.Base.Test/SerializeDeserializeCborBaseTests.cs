using GardasarCode.Base.CBOR;
using GardasarCode.Base.Extensions;
using GardasarCode.Base.Test.Helpers;
using Xunit.Abstractions;

namespace GardasarCode.Base.Test;

public class SerializeDeserializeCborBaseTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void CheckPrimitiveOrNullablePrimitive()
    {
        Assert.True(typeof(int).IsPrimitiveOrNullablePrimitive(), "int should be recognized as primitive.");
        Assert.True(typeof(Double).IsPrimitiveOrNullablePrimitive(), "double should be recognized as primitive.");
        Assert.True(typeof(int?).IsPrimitiveOrNullablePrimitive(), "Nullable<int> should be recognized as nullable primitive.");
        Assert.True(typeof(double?).IsPrimitiveOrNullablePrimitive(), "Nullable<double> should be recognized as nullable primitive.");
        Assert.True(typeof(bool).IsPrimitiveOrNullablePrimitive(), "bool should be recognized as primitive.");
        Assert.False(typeof(string).IsPrimitiveOrNullablePrimitive(), "string should be recognized as primitive.");
        Assert.False(typeof(ExampleStruct).IsPrimitiveOrNullablePrimitive(), "Struct should not be recognized as primitive.");
        Assert.False(typeof(ExampleStruct?).IsPrimitiveOrNullablePrimitive(), "Nullable<struct> should not be recognized as nullable primitive.");
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldISingle()
    {
        // Arrange
        var example = float.MaxValue;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<float>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt()
    {
        // Arrange
        var example = int.MaxValue;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<int>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldString()
    {
        // Arrange
        var example = "Test string";

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<string>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDouble()
    {
        // Arrange
        var example = double.MaxValue;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<double>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldBoolean()
    {
        // Arrange
        var example = true;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<bool>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDateTime()
    {
        // Arrange
        var example = DateTimeOffset.FromUnixTimeSeconds(DateTime.UtcNow.ToUnixTicks()).DateTime;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<DateTime>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDateTimeOffset()
    {
        // Arrange
        var example = DateTimeOffset.FromUnixTimeSeconds(DateTime.UtcNow.ToUnixTicks());

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<DateTimeOffset>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldTimeSpan()
    {
        // Arrange
        var example = TimeSpan.FromSeconds(20);

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<TimeSpan>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldNpgsqlPoint()
    {
        // Arrange
        var example = new NpgsqlTypes.NpgsqlPoint(1.23, 4.56);

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<NpgsqlTypes.NpgsqlPoint>(cborBytes);

        // Assert
        Assert.Equal(example, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldNpgsqlIntervalException()
    {
        // Arrange
        var example = new NpgsqlTypes.NpgsqlInterval(1, 2, 3);

        // Act & Assert
        var cborBytes = CborSerializer.SerializeToCbor(example);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var exception = Assert.Throws<Exception>(() => CborDeserializer.DeserializeFromCbor<NpgsqlTypes.NpgsqlInterval>(cborBytes));
        Assert.Equal("The object of \"NpgsqlInterval\" cannot be restored because there are no writable fields.", exception.Message);
    }
}
