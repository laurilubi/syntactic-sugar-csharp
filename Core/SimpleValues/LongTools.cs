using System;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class LongTools
{
    public static long? GetLongOrNull(this object input)
    {
        if (input == null) return null;
        if (input is long inputLong) return inputLong;
        var success = long.TryParse(input.ToString(), out var value);
        return success
            ? value
            : null;
    }

    public static long GetLongOrDefault(this object input, long defaultValue = default)
    {
        var value = GetLongOrNull(input);
        return value ?? defaultValue;
    }

    public static long GetLongOrThrow(this object input) => GetLongOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as long."));

    public static long GetLongOrThrow<TException>(this object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetLongOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}