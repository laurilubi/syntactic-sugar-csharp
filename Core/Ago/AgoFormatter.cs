using System;
using SyntacticSugar.Core.Ago;

namespace SyntacticSugar.Core;

public interface IAgoFormatter
{
    string Format(TimeAgo ago);
    string Format(TimeSpan ago);
}

public class AgoFormatter : IAgoFormatter
{
    private readonly AgoTranslation transation;

    public AgoFormatter()
    {
        transation = AgoTranslation.English;
    }

    public AgoFormatter(AgoTranslation transation)
    {
        this.transation = transation;
    }

    public string Format(TimeAgo ago) => Format(ago.Ago);

    public string Format(TimeSpan ago)
    {
        var abs = new TimeSpan(Math.Abs(ago.Ticks));
        if (abs.TotalSeconds < 1) return transation.Now;

        var direction = ago.TotalSeconds > 0
            ? transation.Ago
            : transation.InFuture;

        switch (abs.TotalSeconds)
        {
            case < 2: return $"1 {transation.Second} {direction}";
            case < 60: return $"{abs.TotalSeconds:F0} {transation.Seconds} {direction}";
        }

        switch (abs.TotalMinutes)
        {
            case < 2: return $"1 {transation.Minute} {direction}";
            case < 60: return $"{abs.TotalMinutes:F0} {transation.Minutes} {direction}";
        }

        switch (abs.TotalHours)
        {
            case < 2: return $"1 {transation.Hour} {direction}";
            case < 24: return $"{abs.TotalHours:F0} {transation.Hours} {direction}";
        }

        switch (abs.TotalDays)
        {
            case < 2: return $"1 {transation.Day} {direction}";
            case < 30: return $"{abs.TotalDays:F0} {transation.Days} {direction}";
            case < 60: return $"1 {transation.Month} {direction}";
            case < 365: return $"{(abs.TotalDays / 30):F0} {transation.Months} {direction}";
            case < 2 * 365: return $"1 {transation.Year} {direction}";
            default: return $"{(abs.TotalDays / 365):F0} {transation.Years} {direction}";
        }
    }
}