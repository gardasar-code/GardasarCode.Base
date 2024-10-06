using System.Runtime.CompilerServices;

namespace GardasarCode.Base.Extensions;

public static class ExceptionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToHumanizedMessage(this Exception source, string identifier = "")
    {
        if (source is ApplicationException) return $"{source.Message}";

        //return $"{identifier} {source.GetType().Name}: {source.Message}";
        return $"идентификатор ошибки: {identifier}";
    }

    public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
    {
        for (var current = source; canContinue(current); current = nextItem(current)) yield return current;
    }

    public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
    {
        return FromHierarchy(source, nextItem, s => true);
    }

    public static string ToMessageStackTrace(this Exception exception)
    {
        return $"{exception.Message}\n{exception.StackTrace}";
    }
}
