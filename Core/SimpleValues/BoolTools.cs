using System;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class BoolTools
{
    public static bool? GetBoolOrNull(this object input)
    {
        if (input == null) return null;
        if (input is string)
        {
            var inputString = input.ToString().ToLower();
            if (inputString.IsOneOf("y", "yes", "1"))
                input = "true";
            else if (inputString.IsOneOf("n", "no", "0"))
                input = "false";
        }
        else if (input is int inputInt)
        {
            if (inputInt == 1)
                input = "true";
            else if (inputInt == 0)
                input = "false";
        }

        var success = bool.TryParse(input.ToString(), out var value);
        return success
            ? value
            : null;
    }

    public static bool GetBoolOrDefault(this object input, bool defaultValue = default)
    {
        var value = GetBoolOrNull(input);
        return value ?? defaultValue;
    }

    public static bool GetBoolOrThrow(this object input) => GetBoolOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as bool."));

    public static bool GetBoolOrThrow<TException>(this object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetBoolOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}