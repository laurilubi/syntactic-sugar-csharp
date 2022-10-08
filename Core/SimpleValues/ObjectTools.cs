using System;
using System.Collections.Generic;
using System.Linq;

namespace SyntacticSugar.Core.SimpleValues;

public static class ObjectTools
{
    public static bool IsOneOf<TSource>(this TSource item, params TSource[] values) =>
        item.IsOneOf(values.ToList());

    public static bool IsOneOf<TSource>(this TSource item, IEnumerable<TSource> values) =>
        item.IsOneOf(values.ToList());

    public static bool IsOneOf<TSource>(this TSource item, List<TSource> values)
    {
        return values.Contains(item);
    }

    public static bool IsBetween<T>(this T value, T begin, T end, bool inclusive = true)
        where T : IComparable<T>
    {
        AssertTools.Assert(begin != null, () => new ArgumentNullException(nameof(begin)));
        AssertTools.Assert(end != null, () => new ArgumentNullException(nameof(end)));

        return inclusive
            ? begin.CompareTo(value) <= 0 && end.CompareTo(value) >= 0
            : begin.CompareTo(value) < 0 && end.CompareTo(value) > 0;
    }
}