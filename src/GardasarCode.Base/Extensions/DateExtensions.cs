using System.Globalization;

namespace GardasarCode.Base.Extensions;

public static class DateExtensions
{
    /// <summary>
    ///     Проверяет на соответствие формату ISO 8601 - "yyyy-MM-ddTHH:mm:sszzz"
    /// </summary>
    public static bool TryParseISO8601(string dateTimeOffset)
    {
        return DateTimeOffset.TryParseExact(dateTimeOffset, "yyyy-MM-ddTHH:mm:sszzz", null, DateTimeStyles.None, out _);
    }

    /// <summary>
    ///     Парсит к формату ISO 8601 - "yyyy-MM-ddTHH:mm:sszzz"
    /// </summary>
    public static DateTimeOffset ParseISO8601(string dateTimeOffset)
    {
        return DateTimeOffset.ParseExact(dateTimeOffset, "yyyy-MM-ddTHH:mm:sszzz", null, DateTimeStyles.None);
    }

    public static string GetMinDateTimeOffset()
    {
        var d = DateTimeOffset.MinValue;

        return d.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }

    public static string GetMaxDateTimeOffset()
    {
        var d = DateTimeOffset.MaxValue;

        return d.ToString("yyyy-MM-ddTHH:mm:sszzz");
    }

    public static string ToDateTimeRu(this DateTimeOffset dto)
    {
        return dto.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss");
    }

    public static string ToTimeRu(this DateTimeOffset dto)
    {
        return dto.ToString("HH:mm:ss");
    }

    /// <summary>"dd.MM.yyyy HH:mm:ss" </summary>
    public static string ToDateTimeRu(this DateTime date)
    {
        return date.ToString("dd.MM.yyyy HH:mm:ss");
    }

    public static string ToDateTimeRuOffset(this DateTime date)
    {
        return date.ToString("dd.MM.yyyy HH:mm:ss zzz");
    }

    public static string ToDateTimeRuOffset(this DateTime date, TimeSpan offseTimeSpan)
    {
        if (date == new DateTime()) return "";

        return ((DateTimeOffset)date).ToOffset(DateTimeOffset.Now.Offset + offseTimeSpan).ToString("dd.MM.yyyy HH:mm:ss");
    }

    public static string ToDateTimeRuOffset(this DateTimeOffset date)
    {
        return date.ToString("dd.MM.yyyy HH:mm:ss zzz");
    }

    public static string ToDateTimeRuOffset(this DateTimeOffset date, TimeSpan offseTimeSpan, bool @short = true)
    {
        if (date == new DateTimeOffset()) return "";

        return date.ToOffset(date.Offset + offseTimeSpan).ToString(@short ? "dd.MM.yyyy HH:mm:ss" : "dd.MM.yyyy HH:mm:ss zzz");
    }

    public static string ToTimeRu(this DateTime date)
    {
        return date.ToString("HH:mm:ss");
    }

    public static string ToTimeInHourRu(this DateTime date)
    {
        return date.ToString("mm:ss");
    }

    public static string ToTimeRu(this int seconds)
    {
        return seconds >= 3600 ? TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss") : TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    }

    public static string ToDateRu(this DateTimeOffset dto)
    {
        return dto.ToString("dd.MM.yyyy");
    }

    public static string ToDateWithDayName(this DateTime date)
    {
        return date.ToString("dddd, dd MMMM yyyy");
    }

    public static string ToDateTimeISO(this DateTime date)
    {
        return date.ToString("yyyyMMddHHmmss");
    }

    public static string ToShortMonthYearDateRu(this DateTime dto)
    {
        return dto.ToString("dd MMM yy");
    }

    public static string ToDateRu(this DateTime dto)
    {
        return dto.ToString("dd.MM.yyyy");
    }

    public static DateTime ChangeYear(this DateTime dt, int newYear)
    {
        return dt.AddYears(newYear - dt.Year);
    }

    // DateTime myDateSansMilliseconds = myDate.Truncate(TimeSpan.TicksPerSecond);
    // DateTime myDateSansSeconds = myDate.Truncate(TimeSpan.TicksPerMinute)
    // https://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime
    public static DateTime Truncate(this DateTime date, long resolution)
    {
        return new DateTime(date.Ticks - date.Ticks % resolution, date.Kind);
    }

    public static long ToUnixTicks(this DateTime date)
    {
        var timestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
        timestamp /= TimeSpan.TicksPerSecond;

        return timestamp;
    }

    /// <summary> microseconds (16-digit) </summary>
    public static DateTime UnixMicrosecondsToDateTime(this long timestamp, bool local = false)
    {
        var offset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp / 1000);

        return local ? offset.LocalDateTime : offset.UtcDateTime;
    }
}
