using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GardasarCode.Base.Extensions;

/// <summary>
///     Здесь всякие полезные расширения для enum.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    ///     Проверяет, что заданное значение действительно входит в enum.
    /// </summary>
    public static bool IsValid(this Enum enumValue)
    {
        return Enum.IsDefined(enumValue.GetType(), enumValue);
    }

    /// <summary>
    ///     Выдает DisplayAttribute.Name, заданное для элемента enum:
    ///     если для элемента enum задано [Display(Name = "Так я показываюсь в UI")], то выдает "Так я показываюсь в UI".
    ///     Источник:
    ///     https://stackoverflow.com/questions/7966102/how-to-assign-string-values-to-enums-and-use-that-value-in-a-switch/30174850#30174850
    /// </summary>
    public static string? GetDisplayName(this Enum enumValue)
    {
        var attribute = enumValue.GetType()
            //.GetTypeInfo()
            .GetMember(enumValue.ToString()).FirstOrDefault(x => x.MemberType == MemberTypes.Field)?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

        return attribute?.Name;
    }

    /// <summary> Get the Description attribute value for the enum value </summary>
    public static string? GetDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        if (fi is null) return null;

        var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes is { Length: > 0 } ? attributes[0].Description : value.ToString();
    }

    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<TAttribute>();
    }
}
