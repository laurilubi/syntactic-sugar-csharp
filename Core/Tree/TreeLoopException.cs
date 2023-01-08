using System;
using System.Runtime.Serialization;

namespace Syntactic.Sugar.Core.Tree;

public class TreeLoopException : Exception
{
    public TreeLoopException()
    {
    }

    public TreeLoopException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public TreeLoopException(string message)
        : base(message)
    {
    }

    public TreeLoopException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}