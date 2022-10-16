using System;

namespace Syntactic.Sugar.Core.Range;

public static class RangeExtensions
{
    public static bool InRange<T>(this T value, T min, T max, bool inclusive = true)
        where T : IComparable<T>
    {
        AssertTools.Assert(min != null, () => new ArgumentNullException(nameof(min)));
        AssertTools.Assert(max != null, () => new ArgumentNullException(nameof(max)));

        var range = new Range<T>(min, max, inclusive);
        return range.Contains(value);
    }

    public static bool InRange<T>(this T value, Range<T> range)
        where T : IComparable<T>
    {
        AssertTools.Assert(range != null, () => new ArgumentNullException(nameof(range)));
        return range.Contains(value);
    }
}