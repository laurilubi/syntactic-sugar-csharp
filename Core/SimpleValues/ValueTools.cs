using System.Collections.Generic;
using System.Linq;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class ValueTools
{
    public static bool IsOneOf<TSource>(this TSource item, params TSource[] values) =>
        item.IsOneOf(values.ToList());

    public static bool IsOneOf<TSource>(this TSource item, IEnumerable<TSource> values) =>
        item.IsOneOf(values.ToList());

    public static bool IsOneOf<TSource>(this TSource item, List<TSource> values)
    {
        return values.Contains(item);
    }

    public static string FormatWithUnit<T>(this T value, string unit, bool withSpace = true)
    {
        if (value == null) return ""; // TODO use default
        return withSpace
            ? $"{value} {unit}"
            : $"{value}{unit}";
    }
}