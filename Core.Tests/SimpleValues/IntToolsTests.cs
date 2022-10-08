using Syntactic.Sugar.Core.Exceptions;
using Syntactic.Sugar.Core.SimpleValues;
using Xunit;

namespace Core.Tests.SimpleValues;

public class IntToolsTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData(false, null)] // bool
    [InlineData(0, 0)] // int
    [InlineData(1, 1)] // int
    [InlineData(2, 2)] // int
    [InlineData("", null)] // string
    [InlineData(" ", null)] // string
    [InlineData("weird", null)] // string
    [InlineData("true", null)] // string
    public void GetIntOrNull(object input, int? expected)
    {
        var result = input.GetIntOrNull();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("4", 9, 4)]
    [InlineData(null, 9, 9)]
    public void GetIntOrDefault(object input, int defaultValue, int? expected)
    {
        var result = input.GetIntOrDefault(defaultValue);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("4", 4)]
    public void GetIntOrThrow_Returns(object input, int? expected)
    {
        var result = input.GetIntOrThrow();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    public void GetIntOrThrow_Throws(object input)
    {
        Assert.Throws<ModelValidationException>(() => input.GetIntOrThrow());
    }
}