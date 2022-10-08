using Syntactic.Sugar.Core.Exceptions;
using Syntactic.Sugar.Core.SimpleValues;
using Xunit;

namespace Core.Tests.SimpleValues;

public class BoolToolsTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData(false, false)] // bool
    [InlineData(true, true)] // bool
    [InlineData(1, true)] // int
    [InlineData(0, false)] // int
    [InlineData(2, null)] // int
    [InlineData(-2, null)] // int
    [InlineData("", null)] // string
    [InlineData(" ", null)] // string
    [InlineData("weird", null)] // string
    [InlineData("false", false)] // string
    [InlineData("FALSE", false)] // string
    [InlineData("true", true)] // string
    [InlineData("TRUE", true)] // string
    public void GetBoolOrNull(object input, bool? expected)
    {
        var result = input.GetBoolOrNull();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("true", false, true)]
    [InlineData(null, false, false)]
    [InlineData(null, true, true)]
    public void GetBoolOrDefault(object input, bool defaultValue, bool? expected)
    {
        var result = input.GetBoolOrDefault(defaultValue);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("true", true)]
    public void GetBoolOrThrow_Returns(object input, bool? expected)
    {
        var result = input.GetBoolOrThrow();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    public void GetBoolOrThrow_Throws(object input)
    {
        Assert.Throws<ModelValidationException>(() => input.GetBoolOrThrow());
    }
}