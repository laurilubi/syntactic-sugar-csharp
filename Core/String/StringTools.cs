using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Syntactic.Sugar.Core.Collections;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.String;

public static class StringTools
{
    public static bool StringHasValue(
        this object input,
        StringHasValueOptions options = StringHasValueOptions.NotNullEmptyWhitespace)
    {
        var value = input?.ToString();
        return options switch
        {
            StringHasValueOptions.NotNull => value != null,
            StringHasValueOptions.NotNullEmpty => string.IsNullOrEmpty(value),
            StringHasValueOptions.NotNullEmptyWhitespace => string.IsNullOrWhiteSpace(value),
            _ => throw new CodeInconsistencyException()
        };
    }

    public static string GetStringOrDefault(
        this object input,
        string defaultValue = "",
        StringHasValueOptions options = StringHasValueOptions.NotNullEmptyWhitespace)
    {
        return StringHasValue(input, options)
            ? input.ToString()
            : defaultValue;
    }

    public static string GetStringOrThrow(
        this object input,
        StringHasValueOptions options = StringHasValueOptions.NotNullEmptyWhitespace)
    {
        return GetStringOrThrow(input, input =>
        {
            var errorMessage = options switch
            {
                StringHasValueOptions.NotNull => "String value cannot be null.",
                StringHasValueOptions.NotNullEmpty => "String value cannot be null or empty.",
                StringHasValueOptions.NotNullEmptyWhitespace => "String value cannot be null, empty or whitespace.",
                _ => throw new CodeInconsistencyException()
            };
            return new ModelValidationException(errorMessage);
        });
    }

    public static string GetStringOrThrow<TException>(
        this object input,
        Func<object, TException> createException,
        StringHasValueOptions options = StringHasValueOptions.NotNullEmptyWhitespace)
        where TException : Exception
    {
        AssertTools.Assert(StringHasValue(input), () => createException(input));
        return input.ToString();
    }

    public static string Join<T>(this IEnumerable<T> source, string separator = ",")
    {
        return Join(source, x => x, separator);
    }

    public static string Join<T>(this IEnumerable<T> source, Func<T, object> selector, string separator = ",")
    {
        var sb = new StringBuilder();
        source.Select(selector).ForEach((item, index) =>
        {
            if (index > 0) sb.Append(separator);
            sb.Append(item);
        });
        return sb.ToString();
    }

    public static string FirstUppercase(this string input, CultureInfo culture = null)
    {
        culture ??= CultureInfo.InvariantCulture;

        if (string.IsNullOrEmpty(input)) return input;
        return input.Length == 1
            ? input.ToUpper(culture)
            : $"{input.Substring(0, 1).ToUpper(culture)}{input.Substring(1).ToLower(culture)}";
    }

    public static string Truncate(this string input, int maxLength, string suffix = "...")
    {
        AssertTools.Assert(input != null, () => new ArgumentNullException(nameof(input)));
        AssertTools.Assert(suffix != null, () => new ArgumentNullException(nameof(suffix)));
        AssertTools.Assert(suffix.Length <= maxLength, () => new ArgumentException(
            $"{nameof(maxLength)},{nameof(suffix)}",
            $"Parameter {nameof(maxLength)}={maxLength} cannot be less than {nameof(suffix)} length: {suffix}"));

        if (input.Length <= maxLength) return input;
        return input.Substring(0, maxLength - suffix.Length) + suffix;
    }

    public static string InsertAtPositions(this string input, IEnumerable<int> positions, string separator = " ")
    {
        var sortedPositions = positions
            .Where(p => p < input.Length)
            .OrderByDescending(p => p)
            .ToList();
        var sb = new StringBuilder(input);
        foreach (var i in sortedPositions)
        {
            sb.Insert(i, separator);
        }

        return sb.ToString();
    }
}