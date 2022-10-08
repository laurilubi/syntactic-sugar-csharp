namespace SyntacticSugar.Core;

public class AgoTranslation
{
    public string Year { get; }
    public string Years { get; }
    public string Month { get; }
    public string Months { get; }
    public string Day { get; }
    public string Days { get; }

    public string Hour { get; }
    public string Hours { get; }
    public string Minute { get; }
    public string Minutes { get; }
    public string Second { get; }
    public string Seconds { get; }

    public string Now { get; }
    public string Ago { get; }
    public string InFuture { get; }

    public AgoTranslation(
        string year, string month, string day,
        string hour, string minute, string second,
        string now,
        string ago,
        string inFuture,
        string plural = "s",
        string years = null, string months = null, string days = null,
        string hours = null, string minutes = null, string seconds = null)
    {
        Year = year;
        Years = years ?? $"{year}{plural}";
        Month = month;
        Months = months ?? $"{month}{plural}";
        Day = day;
        Days = days ?? $"{day}{plural}";

        Hour = hour;
        Hours = hours ?? $"{hour}{plural}";
        Minute = minute;
        Minutes = minutes ?? $"{minute}{plural}";
        Second = second;
        Seconds = seconds ?? $"{second}{plural}";

        Now = now;
        Ago = ago;
        InFuture = inFuture;
    }

    public static AgoTranslation English => new(
        "year", "month", "day",
        "hour", "minute", "second",
        "now", "ago", "in the future");
}