﻿using System;
using Syntactic.Sugar.Core.Exceptions;

namespace Syntactic.Sugar.Core.SimpleValues;

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
            _ => throw new CodeConsistencyException()
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
                _ => throw new CodeConsistencyException()
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
}