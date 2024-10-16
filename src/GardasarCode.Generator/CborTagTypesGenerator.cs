using System;
using System.Formats.Cbor;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[Generator(LanguageNames.CSharp)]
public class CborTagTypesGenerator : IIncrementalGenerator
{
    private static readonly string[] BaseTypes =
    [
        "Int16", "Int32", "Int64",
        "UInt16", "UInt32", "UInt64",
        "Single", "Double", "Boolean",
        "String", "Byte", "Char",
        "SByte", "Decimal", "DateTime",
        "TimeSpan", "Guid", "DateTimeOffset",

        "Ultimo"
    ];

    private static readonly string[] Prefixes =
    {
        "Cbor"
    };

    private static readonly string[] Suffixes =
    {
        "",
        "Nullable",
        "Array"
    };

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var sourceProvider = context.AnalyzerConfigOptionsProvider.Select((_, _) => GenerateSource());

        context.RegisterSourceOutput(sourceProvider, (ctx, source) => { ctx.AddSource("CborTagTypes.g.cs", SourceText.From(source, Encoding.UTF8)); });
    }

    private static string GenerateSource()
    {
        var startValue = (ulong)CborTag.SelfDescribeCbor;
        var uniqueId = 1;

        var sourceBuilder = new StringBuilder();

        sourceBuilder.AppendLine("using System.Formats.Cbor;");
        sourceBuilder.AppendLine("");
        sourceBuilder.AppendLine("namespace GardasarCode.Generator;");

        sourceBuilder.AppendLine("");
        sourceBuilder.AppendLine("internal static class CborTagTypes");
        sourceBuilder.AppendLine("{");

        foreach (var baseType in BaseTypes)
        foreach (var prefix in Prefixes)
        {
            foreach (var suffix in Suffixes)
            {
                var uniqueValue = startValue - (ulong)uniqueId;
                var fieldName = $"{prefix}{baseType}{suffix}";

                if (string.Equals(fieldName, "CborStringNullable", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (string.Equals(fieldName, "CborUltimoNullable", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (string.Equals(fieldName, "CborUltimoArray", StringComparison.OrdinalIgnoreCase))
                    continue;

                sourceBuilder.AppendLine($"    public static readonly CborTag {fieldName} = (CborTag){uniqueValue};");
                uniqueId++;
            }

            sourceBuilder.AppendLine("");
        }

        sourceBuilder.AppendLine("private const string EmptyByte = \"F6\";");
        sourceBuilder.AppendLine("");

        #region Tag function

        sourceBuilder.AppendLine("public static (CborTag, string?) Tag(Type type)");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine("return type switch");
        sourceBuilder.AppendLine("{");

        foreach (var baseType in BaseTypes)
        {
            if (string.Equals(baseType, "Ultimo", StringComparison.OrdinalIgnoreCase))
                continue;

            foreach (var prefix in Prefixes)
            {
                foreach (var suffix in Suffixes)
                {
                    var fieldName = $"{prefix}{baseType}{suffix}";
                    var suffixType = suffix switch
                    {
                        "Nullable" => $"{baseType}?",
                        "Array" => $"{baseType}[]",
                        _ => baseType
                    };

                    if (string.Equals(fieldName, "CborStringNullable", StringComparison.OrdinalIgnoreCase))
                        continue;

                    sourceBuilder.AppendLine($"    not null when type == typeof({suffixType}) => ({fieldName}, EmptyByte),");
                }

                sourceBuilder.AppendLine("");
            }
        }

        sourceBuilder.AppendLine("    _ => (CborTag.SelfDescribeCbor, $\"{type.FullName}|{type.Assembly.GetName().Name}\")");
        sourceBuilder.AppendLine("};");
        sourceBuilder.AppendLine("}");

        #endregion

        #region UnTag function

        sourceBuilder.AppendLine("public static Type? UnTag(CborTag tag)");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine("return tag switch");
        sourceBuilder.AppendLine("{");

        foreach (var baseType in BaseTypes)
        {
            if (string.Equals(baseType, "Ultimo", StringComparison.OrdinalIgnoreCase))
                continue;

            foreach (var prefix in Prefixes)
            {
                foreach (var suffix in Suffixes)
                {
                    var fieldName = $"{prefix}{baseType}{suffix}";
                    var suffixType = suffix switch
                    {
                        "Nullable" => $"{baseType}?",
                        "Array" => $"{baseType}[]",
                        _ => baseType
                    };

                    if (string.Equals(suffixType, "string?", StringComparison.OrdinalIgnoreCase))
                        continue;

                    sourceBuilder.AppendLine($"    CborTag _ when tag == {fieldName} => typeof({suffixType}),");
                }

                sourceBuilder.AppendLine("");
            }
        }

        sourceBuilder.AppendLine("    _ => null");
        sourceBuilder.AppendLine("};");
        sourceBuilder.AppendLine("}");

        #endregion

        #region SerializeType function

        sourceBuilder.AppendLine("public static bool SerializeType<T>(CborWriter writer, T? obj, Type saveObjectType)");
        sourceBuilder.AppendLine("{");

        foreach (var baseType in BaseTypes)
        {
            if (string.Equals(baseType, "Ultimo", StringComparison.OrdinalIgnoreCase))
                continue;

            sourceBuilder.AppendLine($"if (saveObjectType == typeof({baseType}) && obj is {baseType} _{baseType.ToLower()})");
            sourceBuilder.AppendLine("{");

            var writeAsInt32 = string.Equals(baseType, "Int16", StringComparison.OrdinalIgnoreCase);
            var writeAsUInt32 = string.Equals(baseType, "UInt16", StringComparison.OrdinalIgnoreCase) || string.Equals(baseType, "Byte", StringComparison.OrdinalIgnoreCase) || string.Equals(baseType, "SByte", StringComparison.OrdinalIgnoreCase);
            var writeAsString = string.Equals(baseType, "String", StringComparison.OrdinalIgnoreCase) || string.Equals(baseType, "Char", StringComparison.OrdinalIgnoreCase);

            if (writeAsInt32)
                sourceBuilder.AppendLine($"    writer.WriteInt32((Int32)_{baseType.ToLower()});");
            else if (writeAsUInt32)
                sourceBuilder.AppendLine($"    writer.WriteUInt32((UInt32)_{baseType.ToLower()});");
            else if (writeAsString)
                sourceBuilder.AppendLine($"    writer.WriteTextString(_{baseType.ToLower()}.ToString());");
            else if (string.Equals(baseType, "TimeSpan", StringComparison.OrdinalIgnoreCase))
                sourceBuilder.AppendLine($"    writer.WriteDouble(_{baseType.ToLower()}.TotalMicroseconds);");
            else if (string.Equals(baseType, "DateTime", StringComparison.OrdinalIgnoreCase))
                sourceBuilder.AppendLine($"    writer.WriteUnixTimeSeconds(ToUnixTicks(_{baseType.ToLower()}));");
            else if (string.Equals(baseType, "Guid", StringComparison.OrdinalIgnoreCase))
                sourceBuilder.AppendLine($"    writer.WriteTextString(_{baseType.ToLower()}.ToString());");
            else
                sourceBuilder.AppendLine($"    writer.Write{baseType}(_{baseType.ToLower()});");

            sourceBuilder.AppendLine("    return true;");
            sourceBuilder.AppendLine("}");

            sourceBuilder.AppendLine("");
        }

        sourceBuilder.AppendLine("    return false;");
        sourceBuilder.AppendLine("}");

        #endregion

        #region DeserializeType function

        sourceBuilder.AppendLine("public static object? DeserializeType(CborReader reader, Type readObjectType)");
        sourceBuilder.AppendLine("{");

        foreach (var baseType in BaseTypes)
        {
            if (string.Equals(baseType, "Ultimo", StringComparison.OrdinalIgnoreCase))
                continue;

            var readAsInt32 = string.Equals(baseType, "Int16", StringComparison.OrdinalIgnoreCase) || string.Equals(baseType, "Byte", StringComparison.OrdinalIgnoreCase);

            var readAsUInt32 = string.Equals(baseType, "UInt16", StringComparison.OrdinalIgnoreCase) || string.Equals(baseType, "SByte", StringComparison.OrdinalIgnoreCase) || string.Equals(baseType, "SByte", StringComparison.OrdinalIgnoreCase);

            if (readAsInt32)
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine($"return ({baseType})reader.ReadInt32();");
            }
            else if (readAsUInt32)
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine($"return ({baseType})reader.ReadUInt32();");
            }
            else if (string.Equals(baseType, "String", StringComparison.OrdinalIgnoreCase))
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}))");
                sourceBuilder.AppendLine($"return ({baseType})reader.ReadTextString();");
            }
            else if (string.Equals(baseType, "Char", StringComparison.OrdinalIgnoreCase))
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine($"return ({baseType})reader.ReadTextString()[0];");
            }
            else if (string.Equals(baseType, "TimeSpan", StringComparison.OrdinalIgnoreCase))
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine("return TimeSpan.FromMicroseconds(reader.ReadDouble());");
            }
            else if (string.Equals(baseType, "DateTime", StringComparison.OrdinalIgnoreCase))
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine("return reader.ReadUnixTimeSeconds();");
            }
            else if (string.Equals(baseType, "Guid", StringComparison.OrdinalIgnoreCase))
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine("return new Guid(reader.ReadTextString());");
            }
            else
            {
                sourceBuilder.AppendLine($"if (readObjectType == typeof({baseType}) || readObjectType == typeof({baseType}?))");
                sourceBuilder.AppendLine($"return ({baseType})reader.Read{baseType}();");
            }

            sourceBuilder.AppendLine("");
        }

        sourceBuilder.AppendLine("    return null;");
        sourceBuilder.AppendLine("}");

        #endregion

        sourceBuilder.AppendLine("private static long ToUnixTicks(DateTime date)");
        sourceBuilder.AppendLine("{");
        sourceBuilder.AppendLine("var timestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;");
        sourceBuilder.AppendLine("timestamp /= TimeSpan.TicksPerSecond;");
        sourceBuilder.AppendLine("return timestamp;");
        sourceBuilder.AppendLine("}");

        sourceBuilder.AppendLine("}");

        return sourceBuilder.ToString();
    }
}
