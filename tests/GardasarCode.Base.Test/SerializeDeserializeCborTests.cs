using GardasarCode.Base.CBOR;
using GardasarCode.Base.Test.Helpers;
using Xunit.Abstractions;

namespace GardasarCode.Base.Test;

public class SerializeDeserializeCborTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData(null, null)]
    [InlineData(short.MaxValue, short.MaxValue)]
    [InlineData(ushort.MaxValue, ushort.MaxValue)]
    [InlineData(int.MaxValue, int.MaxValue)]
    [InlineData(uint.MaxValue, uint.MaxValue)]
    [InlineData(long.MaxValue, long.MaxValue)]
    [InlineData(ulong.MaxValue, ulong.MaxValue)]
    [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
    [InlineData(new uint[] { 1, 2, 3 }, new uint[] { 1, 2, 3 })]
    [InlineData(new byte[] { 11, 22, 33 }, new byte[] { 11, 22, 33 })]
    [InlineData(new object[] { 1, "2", null, 4 }, new object[] { 1, "2", null, 4 })]
    public void SerializeDeserializeCbor_ShouldAsObject(object data, object expected)
    {
        // Act
        var cborBytes = CborSerializer.SerializeToCbor(data);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));
        var deserializedValue = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.Equal(expected, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParams()
    {
        // Arrange
        var exampleObject = new ClassWithParams
        {
            Name = "Test",
            Age = 30,
            IsActive = true,
            Tags = ["tag1", "tag2"]
        };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(exampleObject);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));
        var deserializedObject = CborDeserializer.DeserializeFromCbor<ClassWithParams>(cborBytes);

        // Assert
        Assert.Equal(exampleObject.Name, deserializedObject.Name);
        Assert.Equal(exampleObject.Age, deserializedObject.Age);
        Assert.Equal(exampleObject.IsActive, deserializedObject.IsActive);
        Assert.Equal(exampleObject.Tags, deserializedObject.Tags);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAsObject()
    {
        // Arrange
        var exampleObject = new ClassWithParams
        {
            Name = "Test",
            Age = 30,
            IsActive = true,
            Tags = ["tag1", "tag2"]
        };
        object obj = exampleObject;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(obj);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));
        var deserializedObject = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        var result = deserializedObject as ClassWithParams;

        // Assert
        Assert.Equal(exampleObject.Name, result.Name);
        Assert.Equal(exampleObject.Age, result.Age);
        Assert.Equal(exampleObject.IsActive, result.IsActive);
        Assert.Equal(exampleObject.Tags, result.Tags);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldString()
    {
        // Arrange
        var value = "Hello, World!";

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        var deserializedValue = CborDeserializer.DeserializeFromCbor<string>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldStringAsObject()
    {
        // Arrange
        var value = "Hello, World!";
        object obj = value;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(obj);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));
        var deserializedValue = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldBool()
    {
        // Arrange
        var value = true;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        var deserializedValue = CborDeserializer.DeserializeFromCbor<bool>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayInt64()
    {
        // Arrange
        long[] value = { 1, 2, 3, 4 };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        var deserializedValue = CborDeserializer.DeserializeFromCbor<long[]>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32Nullable()
    {
        // Arrange
        int? value = 2;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        var deserializedValue = CborDeserializer.DeserializeFromCbor<int?>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldObject()
    {
        // Arrange
        var @object = new object();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.NotNull(deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldObjectNull()
    {
        // Arrange
        object @object = null;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.Null(deserializedObject);
        Assert.Equal(@object, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32()
    {
        // Arrange
        var exampleInt32 = 1;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(exampleInt32);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<int>(cborBytes);

        // Assert
        Assert.Equal(exampleInt32, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32NullableNull()
    {
        // Arrange
        int? exampleInt32 = null;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(exampleInt32);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<int?>(cborBytes);

        // Assert
        Assert.False(deserializedObject.HasValue);
        Assert.Equal(exampleInt32, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32NullablleNotNull()
    {
        // Arrange
        int? exampleInt32 = 1;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(exampleInt32);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<int?>(cborBytes);

        // Assert
        Assert.True(deserializedObject.HasValue);
        Assert.Equal(exampleInt32, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32AsObject()
    {
        // Arrange
        var exampleInt32 = 1;
        object exampleObjectInt32 = exampleInt32;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(exampleObjectInt32);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.Equal(exampleInt32, deserializedObject);

        Assert.Equal(exampleObjectInt32, deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWinthoutParams()
    {
        // Arrange
        var exampleObject = new ClassWinthoutParams();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(exampleObject);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObjectAsClass = CborDeserializer.DeserializeFromCbor<ClassWinthoutParams>(cborBytes);

        // Assert
        Assert.NotNull(deserializedObjectAsClass);
        Assert.IsType<ClassWinthoutParams>(deserializedObjectAsClass);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWinthoutParamsAsObject()
    {
        // Arrange
        var exampleObject = new ClassWinthoutParams();
        object obj = exampleObject;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(obj);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedObject = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.NotNull(deserializedObject);

        Assert.IsType<ClassWinthoutParams>(deserializedObject);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayInt32()
    {
        // Arrange
        int[] value = { 1, 2, 3, 4 };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<int[]>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayInt32Empty()
    {
        // Arrange
        int[] value = [];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<int[]>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayOfObjects()
    {
        // Arrange
        object[] value = [1, "2", null, 4];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<object[]>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayInt32Nullable()
    {
        // Arrange
        int?[] value = [1, null, null, 4];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<int?[]>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldListInt32()
    {
        // Arrange
        List<int> value = [1, 2, 3, 4];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<List<int>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldListOfObjects()
    {
        // Arrange
        List<object> value = [1, "2", 3, null, "4"];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<List<object>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldListInt32Nullable()
    {
        // Arrange
        List<int?> value = [1, null, null, 4];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<List<int?>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldListInt32NullableIsNull()
    {
        // Arrange
        List<int?> value = null;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<List<int?>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDictionaryInt32Int32()
    {
        // Arrange
        var value = new Dictionary<int, int> { { 1, 1 }, { 2, 2 }, { 3, 3 } };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Dictionary<int, int>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDictionaryInt32Int32Nulable()
    {
        // Arrange
        var value = new Dictionary<int, int?> { { 1, 1 }, { 2, 2 }, { 3, null } };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Dictionary<int, int?>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDictionaryInt32Int32IsNull()
    {
        // Arrange
        Dictionary<int, int> value = null;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Dictionary<int, int>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDictionaryInt32Int32IsEmpty()
    {
        // Arrange
        var value = new Dictionary<int, int>();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Dictionary<int, int>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDictionaryObjectsObjects()
    {
        // Arrange
        var value = new Dictionary<object, object> { { 1, 1 }, { "2", 2 }, { 3, null } };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Dictionary<object, object>>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldDictionaryInt32ClassWithObjectParam()
    {
        // Arrange
        var value = new Dictionary<int, ClassWithObjectParam>
        {
            { 1, new ClassWithObjectParam { Type = "1", Value = (short)1 } },
            { 2, new ClassWithObjectParam { Type = "", Value = null } },
            { 3, new ClassWithObjectParam { Type = null, Value = null } }
        };

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Dictionary<int, ClassWithObjectParam>>(cborBytes);

        // Assert
        //Assert.Equal(value, deserializedValue);
        foreach (var key in value.Keys)
        {
            Assert.Equal(value[key].Type, deserializedValue[key].Type);
            Assert.Equal(value[key].Value, deserializedValue[key].Value);
        }

        Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayBytes()
    {
        // Arrange
        byte[] value = [91, 28, 37, 47];

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<byte[]>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        //Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayBytesAsObject()
    {
        // Arrange
        byte[] value = [91, 28, 37, 47];
        object obj = value;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(obj);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
        //Assert.Equal(value.Count, deserializedValue.Count);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldGuid()
    {
        // Arrange
        var value = Guid.NewGuid();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<Guid>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldGuidAsObject()
    {
        // Arrange
        var value = Guid.NewGuid();
        object obj = value;

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(obj);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var deserializedValue = CborDeserializer.DeserializeFromCbor<object>(cborBytes);

        // Assert
        Assert.Equal(value, deserializedValue);
    }
}
