using Syntactic.Sugar.Core.Range;
using Xunit;

namespace Core.Tests.RangeTests;

public class RangeTests
{
    [Theory]
    [InlineData(10, false)]
    [InlineData(20, true)]
    [InlineData(30, false)]
    public void ContainsInt(int value, bool expected)
    {
        var range = new Range<int>(15, 25);
        Assert.Equal(expected, range.Contains(value));
    }

    [Theory]
    [InlineData("cc", false)]
    [InlineData("hh", true)]
    [InlineData("pp", false)]
    public void ContainsString(string value, bool expected)
    {
        var range = new Range<string>("ee", "mm");
        Assert.Equal(expected, range.Contains(value));
    }

    [Theory]
    [InlineData(15, false, false, false)]
    [InlineData(15, true, false, true)]
    [InlineData(25, false, false, false)]
    [InlineData(25, false, true, true)]
    public void ContainsEdges(int value, bool minInclusive, bool maxInclusive, bool expected)
    {
        var range = new Range<int>(15, 25, minInclusive, maxInclusive);
        Assert.Equal(expected, range.Contains(value));
    }
}