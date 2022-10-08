using System;
using System.Collections.Generic;
using System.Linq;

namespace Syntactic.Sugar.Core.Collections;

public static class ListTools
{
    public static void AddOrSet<TValue>(this List<TValue> items, TValue value, Func<TValue, bool> selector)
    {
        lock (items)
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
    }
}