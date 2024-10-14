[![build](https://github.com/gardasar-code/GardasarCode.Base/actions/workflows/build.yml/badge.svg)](https://github.com/gardasar-code/GardasarCode.Base/actions/workflows/build.yml) [![deploy](https://github.com/gardasar-code/GardasarCode.Base/actions/workflows/deploy.yml/badge.svg)](https://github.com/gardasar-code/GardasarCode.Base/actions/workflows/deploy.yml) [![NuGet Version](https://img.shields.io/nuget/v/GardasarCode.Base.svg)](https://www.nuget.org/packages/GardasarCode.Base/) ![NuGet Downloads](https://img.shields.io/nuget/dt/GardasarCode.Base) ![GitHub License](https://img.shields.io/github/license/gardasar-code/GardasarCode.Base)

# GardasarCode.Base


## CBOR

"Any" object can be serialized to and deserialized from CBOR format using the `CBOR` class.

```csharp
var obj = new  Dictionary<string, object>
{
    { "key1", 1 },
    { "key2", "value2" },
    { "key3", new List<int> { 1, 2, 3 } }
};

CborBytes cbor = CBOR.Serialize(obj);
var obj2 = CBOR.Deserialize<Dictionary<string, object>>(cbor);
```

> **Being supplemented.**
