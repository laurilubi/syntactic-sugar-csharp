using System;

namespace Syntactic.Sugar.Trigo.Model;

/// <summary>
/// 360 degrees, -180 < .. <= 180. Used for angle differences.
/// </summary>
public struct Angle
{
    public readonly double Degree;
    public string Key => $"^{Degree:F3}°";

    public Angle(double degree, bool absolute = false)
    {
        Degree = Normalize(absolute ? Math.Abs(degree) : degree);
    }

    public Angle(Angle orig)
    {
        Degree = Normalize(orig.Degree);
    }

    public double AbsDegree => Math.Abs(Degree);

    public bool IsLeft => Degree < 0; // works based on normalized degree

    public static bool operator ==(Angle a, Angle b) => a.Key == b.Key;
    public static bool operator !=(Angle a, Angle b) => a.Key != b.Key;
    public override string ToString() => Key;

    public static double ToRadian(double degree) => Math.PI * degree / 180d;
    public static double ToDegrees(double radian) => radian * 180d / Math.PI;

    public static double Normalize(double degree, double baseDegree = 0)
    {
        var minBorder = baseDegree - 180;
        var maxBorder = baseDegree + 180;
        while (degree <= minBorder || degree > maxBorder)
        {
            if (degree <= minBorder) degree += 360;
            else if (degree > maxBorder) degree -= 360;
        }

        return degree;
    }

    // public static double GetMissDistance(Point viewer, Point target, double viewerDirectionDegree,
    //     bool absolute = true)
    // {
    //     var distance = Point.GetDistance(viewer, target);
    //     var degreeDiff = GetDegreeDiff(viewer, target, viewerDirectionDegree);
    //     var missDistance = distance * Math.Sin(ToAngle(degreeDiff));
    //     return absolute
    //         ? Math.Abs(missDistance)
    //         : missDistance;
    // }
}