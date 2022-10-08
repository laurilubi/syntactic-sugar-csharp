using System;

namespace Syntactic.Sugar.Core.Exceptions;

[Serializable]
public class DataConsistencyException : Exception
{
    public DataConsistencyException() { }

    public DataConsistencyException(string message) : base(message) { }

    public DataConsistencyException(string message, Exception exception) : base(message, exception) { }
}