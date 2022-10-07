using System;
using System.Collections.Generic;

namespace SyntacticSugar.Core.Collections;

public static class DictionaryTools
{
    public static void TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        try
        {
            dic.Add(key, value);
        }
        catch (ArgumentException ex)
        {
            if (ex is ArgumentNullException) throw;
        }
    }

    public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
    {
        if (dic.ContainsKey(key))
            dic[key] = value;
        else
            dic.Add(key, value);
    }

    /// <returns>List&lt;TValue&gt; to imitate where-selector for lists</returns>
    public static List<TValue> WhereByKey<TKey, TValue>(this IDictionary<TKey, TValue> main, TKey key)
    {
        var success = main.TryGetValue(key, out var value);
        var dictionary = new List<TValue>();
        if (!success) return dictionary;

        dictionary.Add(value);
        return dictionary;
    }

    public static TValue Single<TKey, TValue>(this IDictionary<TKey, TValue> main, TKey key)
    {
        var success = main.TryGetValue(key, out var value);
        AssertTools.Assert(success, () => $"Cannot find key={key} in dictionary.");
        return value;
    }

    public static TValue SingleOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> main, TKey key)
    {
        if (typeof(TKey) == typeof(string) && key == null) return default;

        var success = main.TryGetValue(key, out var value);
        return success
            ? value
            : default;
    }

    /// <summary>
    /// Adds items from second dictionary to the first one.
    /// </summary>
    public static IDictionary<TKey, TValue> AddDictionary<TKey, TValue>(
        this IDictionary<TKey, TValue> main,
        IDictionary<TKey, TValue> second,
        MergeOptions options = MergeOptions.None)
    {
        foreach (var kv in second)
        {
            switch (options)
            {
                case MergeOptions.None:
                    main.Add(kv);
                    break;
                case MergeOptions.PreferMain:
                    if (!main.ContainsKey(kv.Key))
                        main.Add(kv);
                    break;
                case MergeOptions.PreferSecond:
                    if (main.ContainsKey(kv.Key))
                        main[kv.Key] = kv.Value;
                    else
                        main.Add(kv);
                    break;
            }
        }

        return main;
    }

    public enum MergeOptions
    {
        None,
        PreferMain,
        PreferSecond
    }
}