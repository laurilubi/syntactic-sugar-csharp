using System;
using System.Collections.Generic;
using System.Linq;
using Syntactic.Sugar.Core.SimpleValues;

namespace Syntactic.Sugar.Core.Collections;

public static class ListTools
{
    public static void AddOrSet<TValue>(this IList<TValue> items, TValue value, Func<TValue, bool> selector)
    {
        var indexes = items
            .Where(selector)
            .Select((item, index) => index)
            .ToList();
        AssertTools.Assert(indexes.Count < 2,
            () => $"Sequence contains {indexes.Count} matches, but 0 or 1 expected.");

        if (indexes.Count == 0)
            items.Add(value);
        else
            items[indexes[0]] = value;
    }

    public static List<int> SplitInts(this string values, char separator = ',')
    {
        return SplitStrings(values, separator: separator)
            .Select(IntTools.GetIntOrThrow)
            .ToList();
    }

    public static List<long> SplitLongs(this string values, char separator = ',')
    {
        return SplitStrings(values, separator: separator)
            .Select(LongTools.GetLongOrThrow)
            .ToList();
    }

    public static List<string> SplitStrings(this string values, char separator = ',', bool trimItem = true)
    {
        if (string.IsNullOrWhiteSpace(values)) return new List<string>();
        return values
            .Split(separator)
            .Where(x => string.IsNullOrWhiteSpace(x) == false)
            .Select(x => trimItem ? x.Trim() : x)
            .ToList();
    }
}