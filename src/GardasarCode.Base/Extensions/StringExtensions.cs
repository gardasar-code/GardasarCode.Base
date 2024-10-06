using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GardasarCode.Base.Extensions;

/// <summary>
///     Здесь всякие полезные расширения для string.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Добавляет value, если он не пустой, к source с учетом префикса и разделителя.
    ///     Пример использования для построения адреса:
    ///     string address = null;
    ///     address = address.Add("г. ", town, ", ");
    ///     address = address.Add("ул. ", street, ", ");
    ///     address = address.Add("д. ", house, ", ");
    ///     address = address.Add("кв. ", flat, ", ");
    /// </summary>
    public static string Add(this string source, string prefix, string value, string separator)
    {
        if (value.Empty()) return source;

        var result = new StringBuilder(source);

        if (result.Length != 0) result.Append(separator);

        result.Append(prefix);
        result.Append(value);

        return result.ToString();
    }

    // Аналогичный метод для StringBuilder.
    public static StringBuilder Add(this StringBuilder source, string prefix, string value, string separator)
    {
        if (value.Empty()) return source;

        var result = source ?? new StringBuilder();

        if (result.Length != 0) result.Append(separator);

        result.Append(prefix);
        result.Append(value);

        return result;
    }

    // Судя по тестам, этот метод может быть до 9-ти раз быстрее LINQ-метода Contains.
    public static bool Contains(this string source, char value)
    {
        if (source.NotEmpty())
            foreach (var ch in source)
                if (ch == value)
                    return true;

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ContainsIgnoreCase(this string source, string rhs)
    {
        return source.NotEmpty() && source.IndexOf(rhs, StringComparison.CurrentCultureIgnoreCase) >= 0;
    }

    public static string DecodeBase64(this Encoding encoding, string encodedText)
    {
        var textAsBytes = Convert.FromBase64String(encodedText);
        return encoding.GetString(textAsBytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Empty(this string? source)
    {
        return string.IsNullOrEmpty(source);
    }

    public static string? EncodeBase64(this Encoding encoding, string text)
    {
        if (text.Empty()) return null;

        var textAsBytes = encoding.GetBytes(text);

        return Convert.ToBase64String(textAsBytes);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsIgnoreCase(this string source, string rhs)
    {
        return string.Equals(source, rhs, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary> Удваивает одинарные кавычки в строке  </summary>
    /// <param name="source">исходная строка</param>
    /// <returns></returns>
    public static string EscapeSingleQuotes(this string source)
    {
        return string.Join("''", source.Split('\''));
    }

    public static Stream GenerateStreamFromString(this string source)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(source);
        writer.Flush();
        stream.Position = 0;

        return stream;
    }

    /// <summary>
    ///     Выдает подстроку из source, заключенную между startTag и endTag.
    ///     Если в source нет таких тегов, то возвращает null.
    /// </summary>
    public static string? GetSubstring(this string source, string startTag, string endTag, bool includeTags)
    {
        if (source.Empty()) return source;

        var startIdx = 0; // Начальный индекс включительно.

        if (startTag.NotEmpty())
        {
            startIdx = source.IndexOf(startTag, 0, StringComparison.Ordinal);
            if (startIdx >= 0 && !includeTags) startIdx += startTag.Length;
        }

        var endIdx = source.Length; // Конечный индекс НЕ включительно.

        if (startIdx >= 0 && endTag.NotEmpty())
        {
            endIdx = source.IndexOf(endTag, startIdx, StringComparison.Ordinal);

            if (endIdx >= 0 && includeTags) endIdx += endTag.Length;
        }

        return startIdx >= 0 && endIdx >= startIdx ? source.Substring(startIdx, endIdx - startIdx) : null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasInfo(this string source)
    {
        return !string.IsNullOrWhiteSpace(source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEmpty(this string source)
    {
        return !string.IsNullOrEmpty(source);
    }

    public static decimal ToDecimal(this string source, decimal defaultValue = 0)
    {
        if (!decimal.TryParse(source, out var result)) result = defaultValue;

        return result;
    }

    public static int ToInt(this string source, int defaultValue = 0)
    {
        if (!int.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)) result = defaultValue;

        return result;
    }

    public static long ToLong(this string source, long defaultValue = 0)
    {
        if (!long.TryParse(source, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)) result = defaultValue;

        return result;
    }

    /// <summary>
    ///     Конвертирует в Punycode те части доменного имени или URL, которые содержат нелатинские символы.
    ///     Если домен состоит из латинских символов, то останется без изменений.
    ///     Unicode: https://пример.рф/crm/ -> Punycode: https://xn--e1afmkfd.xn--p1ai/crm/
    /// </summary>
    public static string ConvertToPunycode(this string source)
    {
        if (string.IsNullOrEmpty(source)) return string.Empty;

        var idn = new IdnMapping();

        if (Uri.IsWellFormedUriString(source, UriKind.Absolute))
        {
            // Разбиваем URL на компоненты
            var uri = new Uri(source);
            var host = uri.Host;

            // Разбиваем хост на части и кодируем части с нелатинскими символами в формат Punycode
            var hostParts = host.Split('.');

            for (var i = 0; i < hostParts.Length; i++)
                if (hostParts[i].Any(c => c > 127)) // Проверка на наличие нелатинских символов
                    hostParts[i] = idn.GetAscii(hostParts[i]);

            // Объединяем закодированные части хоста
            var punycodeHost = string.Join(".", hostParts);

            // Собираем конечный URL
            var uriBuilder = new UriBuilder(uri) { Host = punycodeHost };

            return uriBuilder.Uri.AbsoluteUri;
        }
        else
        {
            // Обрабатываем как доменное имя
            var hostParts = source.Split('.');

            for (var i = 0; i < hostParts.Length; i++)
                if (hostParts[i].Any(c => c > 127)) // Проверка на наличие нелатинских символов
                    hostParts[i] = idn.GetAscii(hostParts[i]);

            // Объединяем закодированные части хоста
            var punycodeHost = string.Join(".", hostParts);

            return punycodeHost;
        }
    }

    /// <summary>
    ///     Принимает URL или доменное имя и декодирует из Punycode
    ///     Punycode: https://xn--e1afmkfd.xn--p1ai/crm/ -> Unicode: https://пример.рф/crm/
    /// </summary>
    public static string ConvertFromPunycode(this string source)
    {
        if (string.IsNullOrEmpty(source)) return string.Empty;

        var idn = new IdnMapping();

        if (Uri.IsWellFormedUriString(source, UriKind.Absolute))
        {
            // Обрабатываем как полный URL
            var uri = new Uri(source);
            var host = uri.Host;

            // Разбиваем хост на части и декодируем части в формате Punycode
            var hostParts = host.Split('.');

            for (var i = 0; i < hostParts.Length; i++)
                if (hostParts[i].StartsWith("xn--"))
                    hostParts[i] = idn.GetUnicode(hostParts[i]);

            // Объединяем декодированные части хоста
            var decodedHost = string.Join(".", hostParts);

            // Собираем конечный URL
            var uriBuilder = new UriBuilder(uri)
            {
                Host = decodedHost
            };

            return uriBuilder.Uri.AbsoluteUri;
        }
        else
        {
            // Обрабатываем как доменное имя

            var hostParts = source.Split('.');

            for (var i = 0; i < hostParts.Length; i++)
                if (hostParts[i].StartsWith("xn--")) // Проверка на формат Punycode
                    hostParts[i] = idn.GetUnicode(hostParts[i]);

            // Объединяем декодированные части хоста
            var unicodeHost = string.Join(".", hostParts);

            return unicodeHost;
        }
    }

    /// <summary>
    ///     Преобразует объект в XML-строку
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="instance">Объект</param>
    /// <param name="ignoreNamespaces">Игнорировать Namespaces, добавляемые сериалайзером</param>
    /// <returns>XML-строка</returns>
    public static string ToXml<T>(this T instance, bool indent = true, bool ignoreNamespaces = true, string? nameSpace = null, string? url = null, Encoding? encoding = null)
    {
        var stringWriter = new StringWriterWithEncoding(encoding ?? Encoding.UTF8);
        var settings = new XmlWriterSettings { Indent = indent };
        var writer = XmlWriter.Create(stringWriter, settings);

        if (ignoreNamespaces)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            new XmlSerializer(typeof(T)).Serialize(writer, instance, ns);
        }
        else if (nameSpace != null)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add(nameSpace, url);
            new XmlSerializer(typeof(T)).Serialize(writer, instance, ns);
        }
        else
        {
            new XmlSerializer(typeof(T)).Serialize(writer, instance);
        }

        return stringWriter.ToString();
    }

    /// <summary>
    ///     Обрезает заданную строку до заданной длины.
    ///     При addDotsIfTruncated = true заканчивает обрезанную строку троеточием (полный размер строки при этом равен
    ///     length).
    /// </summary>
    public static string? Truncate(this string? source, int length, bool addDotsIfTruncated = false)
    {
        const string dots = "...";

        string? result;

        if (source == null || source.Length <= length)
            result = source;
        else if (addDotsIfTruncated && length > dots.Length)
            result = source.Substring(0, length - dots.Length) + dots;
        else
            result = source.Substring(0, length);

        return result;
    }

    /// <summary>
    ///     Убирает из строки экранирующие символы, делая строку удобной для чтения человеком.
    /// </summary>
    public static string? Unescape(this string source)
    {
        return source.Replace("\\\"", "\"") // \" -> "
            .Replace("\\\\", "\\") // \\ -> \
            .Replace("\\r\\n", "\r\n");
        // \r\n -> перевод строки
    }

    public static string ApiPhoneNumber(this string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        if (value.StartsWith("D")) return "8" + value[5..];

        return value;
    }

    public static Guid ToGuid(this string source)
    {
        Guid.TryParse(source, out var result);
        return result;
    }

    public static string? ToUpperFirstChar(this string source)
    {
        return string.IsNullOrEmpty(source) ? null : $"{char.ToUpper(source[0])}{source.Substring(1)}";
    }

    public sealed class StringWriterWithEncoding(Encoding encoding) : StringWriter
    {
        public override Encoding Encoding { get; } = encoding;
    }
}
