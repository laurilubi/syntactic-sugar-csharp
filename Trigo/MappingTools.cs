using System;

namespace Syntactic.Sugar.Trigo;

public static class MappingTools
{
    public static double Linear(
        Core.Range.Range<double> inputRange,
        Core.Range.Range<double> outputRange,
        double inValue,
        bool cap = true)
    {
        var inputStep = inputRange.Max - inputRange.Min;
        var percent = (inValue - inputRange.Min) / inputStep;
        if (cap)
        {
            if (percent < 0) percent = 0;
            else if (percent > 1) percent = 1;
        }

        var outputStep = outputRange.Max - outputRange.Min;
        return outputRange.Min + (int)Math.Round(outputStep * percent);
    }
}