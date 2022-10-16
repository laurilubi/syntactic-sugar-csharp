using Syntactic.Sugar.Core.Range;
using Xunit;

namespace Core.Tests.RangeTests;

public class RangeExtensionsTests
{
    [Theory]
    [InlineData(10, false, false)]
    [InlineData(15, false, false)]
    [InlineData(15, true, true)]
    [InlineData(20, false, true)]
    [InlineData(25, false, false)]
    [InlineData(25, true, true)]
    [InlineData(30, false, false)]
    public void InRange(int value, bool inclusive, bool expected)
    {
        Assert.Equal(expected, value.InRange(15, 25, inclusive));
    }
}