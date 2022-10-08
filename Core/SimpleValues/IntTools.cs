using System;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class IntTools
{
    public static int? GetIntOrNull(object input)
    {
        if (input == null) return null;
        if (input is int inputInt) return inputInt;
        var success = int.TryParse(input.ToString(), out var value);
        return success
            ? value
            : null;
    }

    public static int GetIntOrDefault(object input, int defaultValue = default)
    {
        var value = GetIntOrNull(input);
        return value ?? defaultValue;
    }

    public static int GetIntOrThrow(object input) => GetIntOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as int."));

    public static int GetIntOrThrow<TException>(object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetIntOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}