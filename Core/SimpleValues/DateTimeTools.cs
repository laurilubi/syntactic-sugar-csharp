using System;
using System.Globalization;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

public static class DateTimeTools
{
    public static DateTime? GetDateOrNull(this object input)
    {
        if (input == null) return null;
        if (input is DateTime inputDate) return inputDate;
        var success = DateTime.TryParse(
            input.ToString(),
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var value);
        return success
            ? value
            : null;
    }

    public static DateTime GetDateOrThrow(this object input) => GetDateOrThrow(input,
        input => new ModelValidationException($"Value {input} cannot be parsed as datetime."));

    public static DateTime GetDateOrThrow<TException>(this object input, Func<object, TException> createException)
        where TException : Exception
    {
        var value = GetDateOrNull(input);
        AssertTools.Assert(value != null, () => createException(input));
        return value.Value;
    }
}