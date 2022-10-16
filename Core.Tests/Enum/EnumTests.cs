using System;
using Syntactic.Sugar.Core.Enum;
using Xunit;

namespace Core.Tests.EnumTests;

public class EnumTests
{
    [Theory]
    [InlineData(TestIntEnum.THREE, "THREE")]
    [InlineData(TestIntEnum.FIVE, "FIVE")]
    public void GetName(TestIntEnum value, string expected)
    {
        var name = value.GetName();
        Assert.Equal(expected, name);
    }

    [Theory]
    [InlineData(TestIntEnum.THREE, "three")]
    [InlineData(TestIntEnum.FIVE, "five")]
    public void GetDescription(TestIntEnum value, string expected)
    {
        var name = value.GetDescription();
        Assert.Equal(expected, name);
    }

    [Theory]
    [InlineData(TestIntEnum.THREE, "three")]
    [InlineData(TestIntEnum.FIVE, "five")]
    [InlineData(TestIntEnum.EIGHT, "EIGHT")]
    public void GetDescriptionOrName(TestIntEnum value, string expected)
    {
        var name = value.GetDescriptionOrName();
        Assert.Equal(expected, name);
    }

    [Theory]
    [InlineData("THREE", TestIntEnum.THREE)]
    [InlineData("FIVE", TestIntEnum.FIVE)]
    public void FromName(string value, TestIntEnum expected)
    {
        var enumValue = EnumTools.FromName<TestIntEnum>(value);
        Assert.Equal(expected, enumValue);
    }

    [Theory]
    [InlineData("THIRTY_THREE", TestLongEnum.THIRTY_THREE)]
    [InlineData("FIFTY_FIVE", TestLongEnum.FIFTY_FIVE)]
    public void Long_FromName(string value, TestLongEnum expected)
    {
        var enumValue = EnumTools.FromName<TestLongEnum>(value);
        Assert.Equal(expected, enumValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("three")]
    [InlineData("OTHER")]
    public void Invalid_FromName_Throws(string value)
    {
        Assert.Throws<ArgumentException>(() => EnumTools.FromName<TestIntEnum>(value));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("THREE", TestIntEnum.THREE)]
    [InlineData("FIVE", TestIntEnum.FIVE)]
    public void FromNameNullable(string value, TestIntEnum? expected)
    {
        var enumValue = EnumTools.FromNameNullable<TestIntEnum>(value);
        Assert.Equal(expected, enumValue);
    }

    [Theory]
    [InlineData("OTHER")]
    public void Invalid_FromNameNullable_Throws(string value)
    {
        Assert.Throws<ArgumentException>(() => EnumTools.FromNameNullable<TestIntEnum>(value));
    }

    [Theory]
    [InlineData("three", TestIntEnum.THREE)]
    [InlineData("five", TestIntEnum.FIVE)]
    public void FromDescription(string value, TestIntEnum expected)
    {
        var enumValue = EnumTools.FromDescription<TestIntEnum>(value);
        Assert.Equal(expected, enumValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("THREE")]
    [InlineData("other")]
    public void Invalid_FromDescription_Throws(string value)
    {
        Assert.Throws<ArgumentException>(() => EnumTools.FromDescription<TestIntEnum>(value));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("three", TestIntEnum.THREE)]
    [InlineData("five", TestIntEnum.FIVE)]
    public void FromDescriptionNullable(string value, TestIntEnum? expected)
    {
        var enumValue = EnumTools.FromDescriptionNullable<TestIntEnum>(value);
        Assert.Equal(expected, enumValue);
    }

    [Theory]
    [InlineData("other")]
    public void Invalid_FromDescriptionNullable_Throws(string value)
    {
        Assert.Throws<ArgumentException>(() => EnumTools.FromDescriptionNullable<TestIntEnum>(value));
    }

    [Fact]
    public void GetValues()
    {
        var values = EnumTools.GetValues<TestIntEnum>();

        Assert.Equal(3, values.Count);

        Assert.Equal(TestIntEnum.THREE, values[0].Value);
        Assert.Equal("THREE", values[0].Name);
        Assert.Equal("three", values[0].Description);

        Assert.Equal(TestIntEnum.FIVE, values[1].Value);
        Assert.Equal("FIVE", values[1].Name);
        Assert.Equal("five", values[1].Description);

        Assert.Equal(TestIntEnum.EIGHT, values[2].Value);
        Assert.Equal("EIGHT", values[2].Name);
        Assert.Equal(null, values[2].Description);
    }
}