using System;

namespace Syntactic.Sugar.Trigo.Model;

/// <summary>
/// 360 degrees. By default -180 to 180 values, 0 facing to the right, 90 down.
/// </summary>
public struct Direction
{
    public readonly double Degree;
    public string Key => $"^{Degree:F3}°";

    public Direction(double degree)
    {
        Degree = Angle.Normalize(degree);
    }

    public Direction(Direction orig)
    {
        Degree = Angle.Normalize(orig.Degree);
    }

    public Direction(Point start, Point end)
    {
        Degree = GetDirection(start, end).Degree;
    }

    public static bool operator ==(Direction a, Direction b) => a.Key == b.Key;
    public static bool operator !=(Direction a, Direction b) => a.Key != b.Key;
    public override string ToString() => Key;

    public static Direction GetDirection(Point start, Point end)
    {
        var radian = Math.Atan2(end.Y - start.Y, end.X - start.X);
        return new Direction(Angle.ToDegrees(radian));
    }

    public static Angle Diff(Direction a, Direction b, bool absolute = false)
    {
        var diff = Angle.Normalize(b.Degree - a.Degree);
        return new Angle(diff, absolute);
    }

    public static Direction Between(Direction a, Direction b, double aWeight = 1, double bWeight = 1)
    {
        var totalWeight = aWeight + bWeight;
        if (totalWeight == 0) throw new ArgumentException($"Total weight cannot be zero: {aWeight}, {bWeight}");

        var bWeightRatio = bWeight / totalWeight;
        var bRebased = new Angle(b.Degree - a.Degree);

        var betweenRebased = new Angle(bRebased.Degree * bWeightRatio);
        var between = new Direction(betweenRebased.Degree + a.Degree);
        return between;
    }
}