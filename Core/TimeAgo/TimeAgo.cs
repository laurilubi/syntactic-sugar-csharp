using System;

namespace Syntactic.Sugar.Core.TimeAgo;

public class TimeAgo
{
    public DateTime Stamp { get; }
    public TimeSpan Ago => _nowFunc() - Stamp;

    private readonly Func<DateTime> _nowFunc;

    public TimeAgo(DateTime stamp, DateTime now)
    {
        AssertTools.Assert(stamp.Kind == now.Kind, () => new ArgumentException(
            $"Times are expected to have same Kind, but have {stamp.Kind} vs {now.Kind}"));

        Stamp = stamp;
        _nowFunc = () => now;
    }

    public TimeAgo(DateTime stamp, bool fixedNow = true)
    {
        AssertTools.Assert(stamp.Kind != DateTimeKind.Unspecified, () => new ArgumentException(
            "DateTimeKind.Unspecified not supported", nameof(stamp)));

        Stamp = stamp;
        if (fixedNow)
        {
            var fixedNowValue = stamp.Kind == DateTimeKind.Utc
                ? DateTime.UtcNow
                : DateTime.Now;
            _nowFunc = () => fixedNowValue;
        }
        else
        {
            _nowFunc = stamp.Kind == DateTimeKind.Utc
                ? () => DateTime.UtcNow
                : () => DateTime.Now;
        }
    }
}