using System;
using System.Globalization;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class DoubleTools
{
    public static double? GetDoubleOrNull(object input)
    {
        if (input == null) return null;
        if (input is double inputDouble) return inputDouble;
        var success = double.TryParse(
            input.ToString().Replace(',', '.'),
            NumberStyles.Float | NumberStyles.AllowThousands,
            NumberFormatInfo.InvariantInfo,
            out var value);
        return success
            ? value
            : (double?)null;
    }

    public static double GetDoubleOrDefault(object input, double defaultValue = default)
    {
        var value = GetDoubleOrNull(input);
        return value ?? defaultValue;
    }

    public static double GetDoubleOrThrow(object input) => GetDoubleOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as double."));

    public static double GetDoubleOrThrow<TException>(object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetDoubleOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}