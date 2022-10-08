using System;
using System.Collections.Generic;
using System.Linq;

namespace Syntactic.Sugar.Core.Collections;

public static class EnumerableTools
{
    /// <summary>
    /// Overload for IEnumerable
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        source.ToList().ForEach(action);
    }

    /// <summary>
    /// Loop and keep track of element index
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        var index = 0;
        foreach (var item in source) action(item, index++);
    }
    
    /// <summary>
    /// Convert IEnumerable&lt;KeyValuePair&lt;TKey, TValue&gt;&gt; to Dictionary&lt;TKey, TValue&gt;
    /// </summary>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source)
    {
        return source.ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Convert IEnumerable&lt;IGrouping&lt;TKey, TValue&gt;&gt; to Dictionary&lt;TKey, List&lt;TValue&gt;
    /// </summary>
    public static Dictionary<TKey, List<TValue>> ToDictionary<TKey, TValue>(
        this IEnumerable<IGrouping<TKey, TValue>> source)
    {
        return source.ToDictionary(x => x.Key, x => new List<TValue>(x));
    }

    /// <summary>
    /// Distinct with selector
    /// </summary>
    public static IEnumerable<TSource> Distinct<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.GroupBy(selector).Select(x => x.First());
    }

    public static IEnumerable<IGrouping<int, T>> GroupByCount<T>(this IEnumerable<T> items, int splitByCount)
    {
        var groups = items
            .Select((x, index) => new { Index = index / splitByCount, Value = x })
            .GroupBy(_ => _.Index, _ => _.Value);
        return groups;
    }

    public static TSource RandomOrDefault<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null) return default;
        if (source.Any() == false) return default;

        var sourceList = source.ToList();
        if (sourceList.Count == 1) return sourceList[0];

        var random = new Random();
        return sourceList[random.Next(sourceList.Count)];
    }
}