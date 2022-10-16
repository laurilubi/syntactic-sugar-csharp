using System;

namespace Syntactic.Sugar.Core.Enum;

public class SimpleEnumValue
{
    public ValueType Value { get; }
    public string Name { get; }
    public string Description { get; }

    public SimpleEnumValue(ValueType value, string name, string description)
    {
        Value = value;
        Name = name;
        Description = description;
    }
}