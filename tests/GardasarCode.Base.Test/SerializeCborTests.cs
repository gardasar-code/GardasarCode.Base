using GardasarCode.Base.CBOR;
using GardasarCode.Base.Test.Helpers;
using Xunit.Abstractions;

namespace GardasarCode.Base.Test;

public class SerializeCborTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void SerializeDeserializeCbor_ShouldObject()
    {
        // Arrange
        var @object = new object();

        var byteString = "F7";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldObjectNull()
    {
        // Arrange
        object @object = null;

        var byteString = "F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWinthoutParamsNullAsObject()
    {
        // Arrange
        ClassWinthoutParams value = null;
        object @object = value;

        var byteString = "F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWinthoutParamsAsSelf()
    {
        // Arrange
        var value = new ClassWinthoutParams();

        var byteString = "F7";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWinthoutParamsAsObject()
    {
        // Arrange
        var value = new ClassWinthoutParams();
        object @object = value;

        var byteString = "D9-D9-F7-A1-78-49-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-2E-48-65-6C-70-65-72-73-2E-43-6C-61-73-73-57-69-6E-74-68-6F-75-74-50-61-72-61-6D-73-7C-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-F7";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);

        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        var cborBytescborBytes = CborDeserializer.DeserializeFromCbor<object>(@cborBytes);
        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32asSelf()
    {
        // Arrange
        var value = 10;

        var byteString = "0A";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32asObject()
    {
        // Arrange
        var value = 10;
        object @object = value;

        var byteString = "D9-D9-F2-A1-62-46-36-0A";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }


    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32NullableAsSelf()
    {
        // Arrange
        int? value = 10;

        var byteString = "0A";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32NullableNullAsSelf()
    {
        // Arrange
        int? value = null;

        var byteString = "F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32NullableAsObject()
    {
        // Arrange
        int? value = 10;
        object @object = value;

        var byteString = "D9-D9-F2-A1-62-46-36-0A";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }


    [Fact]
    public void SerializeDeserializeCbor_ShouldInt32NullableNullAsObject()
    {
        // Arrange
        int? value = null;

        var byteString = "F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayNullAsSelf()
    {
        // Arrange
        int[] value = null;

        var byteString = "F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayNullAsObject()
    {
        // Arrange
        int[] value = null;

        var byteString = "F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayEmptyAsSelf()
    {
        // Arrange
        int[] value = [];

        var byteString = "80";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayEmptyAsObject()
    {
        // Arrange
        int[] value = [];
        object @object = value;

        var byteString = "D9-D9-F0-A1-62-46-36-80";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldArrayAsSelf()
    {
        // Arrange
        int[] value = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

        var byteString = "98-3C-01-02-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10-11-12-13-14-01-02-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10-11-12-13-14-01-02-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10-11-12-13-14";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAsObject()
    {
        // Arrange
        var value = new ClassWithParams();
        object @object = value;

        var byteString = "D9-D9-F7-A1-78-45-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-2E-48-65-6C-70-65-72-73-2E-43-6C-61-73-73-57-69-74-68-50-61-72-61-6D-73-7C-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-A4-64-4E-61-6D-65-F6-63-41-67-65-00-68-49-73-41-63-74-69-76-65-F4-64-54-61-67-73-F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAsSelf()
    {
        // Arrange
        var value = new ClassWithParams();

        var byteString = "A4-64-4E-61-6D-65-F6-63-41-67-65-00-68-49-73-41-63-74-69-76-65-F4-64-54-61-67-73-F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAndClassWithParamsAsSelfInnerNullAsSelf()
    {
        // Arrange
        var value = new ClassWithParamsAndClassWithParamsAsSelf();

        var byteString = "A2-64-4E-61-6D-65-F6-6A-49-6E-6E-65-72-43-6C-61-73-73-F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAndClassWithParamsAsSelfInnerNotNullAsSelf()
    {
        // Arrange
        var value = new ClassWithParamsAndClassWithParamsAsSelf { Name = "Test", InnerClass = new ClassWithParams { Name = "INTest", IsActive = true, Tags = ["1", "2"] } };

        var byteString = "A2-64-4E-61-6D-65-64-54-65-73-74-6A-49-6E-6E-65-72-43-6C-61-73-73-A4-64-4E-61-6D-65-66-49-4E-54-65-73-74-63-41-67-65-00-68-49-73-41-63-74-69-76-65-F5-64-54-61-67-73-82-61-31-61-32";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithArrayNullAsSelf()
    {
        // Arrange
        var value = new ClassWithArray();

        var byteString = "A1-64-54-61-67-73-F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithArrayNotNullAsSelf()
    {
        // Arrange
        var value = new ClassWithArray { Tags = [1, 2, 3, 4, 5, 6, 7] };

        var byteString = "A1-64-54-61-67-73-87-01-02-03-04-05-06-07";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithArrayEmptyAsSelf()
    {
        // Arrange
        var value = new ClassWithArray { Tags = [] };

        var byteString = "A1-64-54-61-67-73-80";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithArrayNotNullAsObject()
    {
        // Arrange
        var value = new ClassWithArray { Tags = [1, 2, 3, 4, 5, 6, 7] };
        object @object = value;

        var byteString = "D9-D9-F7-A1-78-44-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-2E-48-65-6C-70-65-72-73-2E-43-6C-61-73-73-57-69-74-68-41-72-72-61-79-7C-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-A1-64-54-61-67-73-87-01-02-03-04-05-06-07";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAndClassWithParamsAsSelfAsSelf()
    {
        // Arrange
        var value = new ClassWithParamsAndClassWithParamsAsSelf();

        var byteString = "A2-64-4E-61-6D-65-F6-6A-49-6E-6E-65-72-43-6C-61-73-73-F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(value);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }

    [Fact]
    public void SerializeDeserializeCbor_ShouldClassWithParamsAndClassWithParamsAsSelfAsObject()
    {
        // Arrange
        var value = new ClassWithParamsAndClassWithParamsAsSelf();
        object @object = value;

        var byteString = "D9-D9-F7-A1-78-5D-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-2E-48-65-6C-70-65-72-73-2E-43-6C-61-73-73-57-69-74-68-50-61-72-61-6D-73-41-6E-64-43-6C-61-73-73-57-69-74-68-50-61-72-61-6D-73-41-73-53-65-6C-66-7C-47-61-72-64-61-73-61-72-43-6F-64-65-2E-42-61-73-65-2E-54-65-73-74-A2-64-4E-61-6D-65-F6-6A-49-6E-6E-65-72-43-6C-61-73-73-F6";
        var byteArray = byteString.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray();

        // Act
        var cborBytes = CborSerializer.SerializeToCbor(@object);
        testOutputHelper.WriteLine(BitConverter.ToString(cborBytes));

        // Assert
        Assert.NotNull(cborBytes);
        Assert.Equal(byteArray, cborBytes);
    }
}
