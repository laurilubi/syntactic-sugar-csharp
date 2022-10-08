using System;

namespace Syntactic.Sugar.Core.Ago;

public interface IAgoFormatter
{
    string Format(TimeAgo ago);
    string Format(TimeSpan ago);
}

public class AgoFormatter : IAgoFormatter
{
    private readonly AgoTranslation _transation;

    public AgoFormatter()
    {
        _transation = AgoTranslation.English;
    }

    public AgoFormatter(AgoTranslation transation)
    {
        this._transation = transation;
    }

    public string Format(TimeAgo ago) => Format(ago.Ago);

    public string Format(TimeSpan ago)
    {
        var abs = new TimeSpan(Math.Abs(ago.Ticks));
        if (abs.TotalSeconds < 1) return _transation.Now;

        var direction = ago.TotalSeconds > 0
            ? _transation.Ago
            : _transation.InFuture;

        switch (abs.TotalSeconds)
        {
            case < 2: return $"1 {_transation.Second} {direction}";
            case < 60: return $"{abs.TotalSeconds:F0} {_transation.Seconds} {direction}";
        }

        switch (abs.TotalMinutes)
        {
            case < 2: return $"1 {_transation.Minute} {direction}";
            case < 60: return $"{abs.TotalMinutes:F0} {_transation.Minutes} {direction}";
        }

        switch (abs.TotalHours)
        {
            case < 2: return $"1 {_transation.Hour} {direction}";
            case < 24: return $"{abs.TotalHours:F0} {_transation.Hours} {direction}";
        }

        switch (abs.TotalDays)
        {
            case < 2: return $"1 {_transation.Day} {direction}";
            case < 30: return $"{abs.TotalDays:F0} {_transation.Days} {direction}";
            case < 60: return $"1 {_transation.Month} {direction}";
            case < 365: return $"{(abs.TotalDays / 30):F0} {_transation.Months} {direction}";
            case < 2 * 365: return $"1 {_transation.Year} {direction}";
            default: return $"{(abs.TotalDays / 365):F0} {_transation.Years} {direction}";
        }
    }
}