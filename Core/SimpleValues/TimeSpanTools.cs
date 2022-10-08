using System;
using System.Globalization;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class TimeSpanTools
{
    public static TimeSpan? GetTimeOrNull(this object input)
    {
        if (input == null) return null;
        if (input is TimeSpan inputTime) return inputTime;
        var success = TimeSpan.TryParse(
            input.ToString(),
            NumberFormatInfo.InvariantInfo,
            out var value);
        return success
            ? value
            : null;
    }

    public static TimeSpan GetTimeOrThrow(this object input) => GetTimeOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as timespan."));

    public static TimeSpan GetTimeOrThrow<TException>(this object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetTimeOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}