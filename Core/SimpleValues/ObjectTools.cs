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
}