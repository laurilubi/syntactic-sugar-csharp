﻿namespace Syntactic.Sugar.Core.SimpleValues;

public enum StringHasValueOptions
{
    NotNull,

    /// <summary>
    /// Not null nor empty string.
    /// </summary>
    NotNullEmpty,

    /// <summary>
    /// Not null, empty string or whitespace (according to string.IsNullOrWhitespace method).
    /// </summary>
    NotNullEmptyWhitespace,
}