using System;

namespace Syntactic.Sugar.Core.Range;

public class Range<T>
    where T : IComparable<T>
{
    public T Min { get; }
    public T Max { get; }
    public bool MinInclusive { get; } = true;
    public bool MaxInclusive { get; } = true;

    public Range(T min, T max)
    {
        AssertTools.Assert(min != null, () => new ArgumentNullException(nameof(min)));
        AssertTools.Assert(max != null, () => new ArgumentNullException(nameof(max)));

        Min = min;
        Max = max;
    }

    public Range(T min, T max, bool inclusive)
        : this(min, max)
    {
        MinInclusive = inclusive;
        MaxInclusive = inclusive;
    }

    public Range(T min, T max, bool minInclusive, bool maxInclusive)
        : this(min, max)
    {
        MinInclusive = minInclusive;
        MaxInclusive = maxInclusive;
    }

    public bool Contains(T item)
    {
        var minComparison = Min.CompareTo(item);
        if (minComparison > 0) return false;
        if (MinInclusive == false && minComparison == 0) return false;

        var maxComparison = Max.CompareTo(item);
        if (maxComparison < 0) return false;
        if (MaxInclusive == false && maxComparison == 0) return false;

        return true;
    }
}