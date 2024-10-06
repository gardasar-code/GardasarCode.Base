using System.Runtime.CompilerServices;

namespace GardasarCode.Base.Extensions;

/// <summary>
///     Здесь всякие полезные расширения для массивов и т.п.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    ///     Этот метод предназначен для использования с ICollection-типами (т.е. у которых есть свойство Count) вместо
    ///     LINQ-метода Any.
    ///     Судя по тестам, этот метод примерно в 5 раз быстрее LINQ-метода Any.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NotEmpty<T>(this ICollection<T> source)
    {
        return source != null && source.Count != 0;
    }

    // Для сравнения код LINQ-метода Any (получен через ILSpy):
    // -----------------------------------------------------------
    // public static bool Any<TSource>(this IEnumerable<TSource> source)
    // {
    //     if (source == null)
    //     {
    //         throw Error.ArgumentNull("source");
    //     }
    //     using (IEnumerator<TSource> enumerator = source.GetEnumerator())
    //     {
    //         if (enumerator.MoveNext())
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    // -----------------------------------------------------------
    /// <summary>
    ///     Вызывает метод action для всех элементов коллекции collection (аналог List_T_.ForEach).
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection) action(item);
    }

    // быстрая итерация в обратном порядке
    public static IEnumerable<T> FastReverse<T>(this IList<T> items)
    {
        for (var i = items.Count - 1; i >= 0; i--) yield return items[i];
    }

    public static bool Any<T>(this T[]? source, Func<T, bool> predicate)
    {
        if (source != null && source.Length != 0)
            foreach (var item in source)
                if (predicate(item))
                    return true;

        return false;
    }

    /// <summary> Перемешивает элементы списка </summary>
    public static IEnumerable<T> ShuffleList<T>(this IList<T> list)
    {
        var random = new Random();

        var n = list.Count;

        // Проходим по списку с конца к началу
        for (var i = n - 1; i > 0; i--)
        {
            // Генерируем случайный индекс от 0 до i (включительно)
            var randomIndex = random.Next(0, i + 1);

            // Меняем местами элементы с индексами i и randomIndex
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }

        return list;
    }

    /// <summary>
    ///     Дополняет данные одного словаря данными из другого по ключу.
    ///     Не меняет значения существующих ключей.
    /// </summary>
    public static Dictionary<TKey, TValue> UpdateDictionary<TKey, TValue>(this Dictionary<TKey, TValue> constDictionary, Dictionary<TKey, TValue> sourceDictionary) where TKey : notnull
    {
        foreach (var pair in sourceDictionary.Where(keyValuePair => !constDictionary.ContainsKey(keyValuePair.Key))) constDictionary.Add(pair.Key, pair.Value);

        return constDictionary;
    }
}
