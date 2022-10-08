using System;
using System.Globalization;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class DecimalTools
{
    public static decimal? GetDecimalOrNull(this object input)
    {
        if (input == null) return null;
        if (input is decimal inputDecimal) return inputDecimal;
        var success = decimal.TryParse(
            input.ToString().Replace(',', '.'),
            NumberStyles.Number,
            NumberFormatInfo.InvariantInfo,
            out var value);
        return success
            ? value
            : null;
    }

    public static decimal GetDecimalOrDefault(this object input, decimal defaultValue = default)
    {
        var value = GetDecimalOrNull(input);
        return value ?? defaultValue;
    }

    public static decimal GetDecimalOrThrow(this object input) => GetDecimalOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as decimal."));

    public static decimal GetDecimalOrThrow<TException>(this object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetDecimalOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}