using System.ComponentModel;

namespace Core.Tests.EnumTests;

public enum TestIntEnum : int
{
    [Description("three")] THREE = 3,
    [Description("five")] FIVE = 5,
    EIGHT = 8,
}

public enum TestLongEnum : long
{
    [Description("thirty three")] THIRTY_THREE = 33,
    [Description("fifty five")] FIFTY_FIVE = 55,
}