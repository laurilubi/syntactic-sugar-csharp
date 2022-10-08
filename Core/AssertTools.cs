using System;
using JetBrains.Annotations;

namespace Syntactic.Sugar.Core;

public static class AssertTools
{
    [ContractAnnotation("halt <= condition: false")]
    public static void Assert(bool condition)
    {
        if (condition) return;
        throw new Exception();
    }

    [ContractAnnotation("halt <= condition: false")]
    public static void Assert(bool condition, Func<string> createMessage)
    {
        if (condition) return;
        throw new Exception(createMessage());
    }

    [ContractAnnotation("halt <= condition: false")]
    public static void Assert<TException>(bool condition, Func<TException> createException)
        where TException : Exception
    {
        if (condition) return;
        throw createException();
    }
}