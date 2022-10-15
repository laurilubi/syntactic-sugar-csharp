using System;
using System.Threading;
using Newtonsoft.Json;
using Syntactic.Sugar.Core.TimeAgo;
using Xunit;

namespace Core.Tests.TimeAgoTests;

public class TimeAgoTests
{
    [Fact]
    public void IsUsableForJson()
    {
        var stamp = new DateTime(2022, 10, 14, 14, 11, 3, DateTimeKind.Utc);
        var now = new DateTime(2022, 10, 15, 17, 33, 44, DateTimeKind.Utc);

        var timeago = new TimeAgo(stamp, now);
        var expected = new
        {
            Stamp = "2022-10-14T14:11:03Z",
            Ago = "1.03:22:41"
        };
        Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(timeago));
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    [InlineData(DateTimeKind.Utc)]
    public void WithSpecifiedNow(DateTimeKind kind)
    {
        var stamp = new DateTime(2022, 10, 14, 14, 0, 0, kind);
        var now = new DateTime(2022, 10, 15, 14, 0, 0, kind);

        var ago = new TimeAgo(stamp, now);
        Assert.Equal(new TimeSpan(24, 0, 0), ago.Ago);
    }

    [Theory]
    [InlineData(DateTimeKind.Local, DateTimeKind.Unspecified)]
    [InlineData(DateTimeKind.Local, DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Unspecified, DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified, DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Utc, DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc, DateTimeKind.Unspecified)]
    public void WithSpecifiedNow_MixedKind_Throws(DateTimeKind stampKind, DateTimeKind nowKind)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var stamp = new DateTime(2022, 10, 14, 14, 0, 0, stampKind);
            var now = new DateTime(2022, 10, 15, 14, 0, 0, nowKind);

            new TimeAgo(stamp, now);
        });
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void WithAutoNow_UnspecifiedKind_Throws(bool fixedNow)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            var stamp = new DateTime(2022, 10, 14, 14, 0, 0, DateTimeKind.Unspecified);
            new TimeAgo(stamp, fixedNow);
        });
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    public void WithFixedNow(DateTimeKind stampKind)
    {
        var stamp = new DateTime(2022, 10, 14, 14, 0, 0, stampKind);
        var timeago = new TimeAgo(stamp, fixedNow: true);
        var ago = timeago.Ago;

        Thread.Sleep(1000);

        var laterAgo = timeago.Ago;
        Assert.Equal(ago, laterAgo);
    }


    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    public void WithGlidingNow(DateTimeKind stampKind)
    {
        var stamp = new DateTime(2022, 10, 14, 14, 0, 0, stampKind);
        var timeago = new TimeAgo(stamp, fixedNow: false);
        var ago = timeago.Ago;

        Thread.Sleep(1000);

        var laterAgo = timeago.Ago;
        Assert.InRange(
            laterAgo,
            ago.Add(TimeSpan.FromMilliseconds(900)),
            ago.Add(TimeSpan.FromMilliseconds(1100)));
    }
}