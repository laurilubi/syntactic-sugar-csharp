using System;

namespace Syntactic.Sugar.Trigo.Model;

public struct MapVector
{
    public readonly Point Start;
    public readonly Point End;
    private double? lengthCache;
    private Direction? directionCache;
    public string Key => $"{Start.Key}-{End.Key}";

    public MapVector(Point end)
    {
        Start = new Point(0, 0);
        End = new Point(end);
        lengthCache = null;
        directionCache = null;
    }

    public MapVector(Point start, Point end)
    {
        Start = new Point(start);
        End = new Point(end);
        lengthCache = null;
        directionCache = null;
    }

    public MapVector(Point start, Vector vector)
    {
        Start = new Point(start);
        End = new Point(start.X + vector.VX, start.Y + vector.VY);
        lengthCache = null;
        directionCache = null;
    }

    public MapVector(int endX, int endY)
    {
        Start = new Point(0, 0);
        End = new Point(endX, endY);
        lengthCache = null;
        directionCache = null;
    }

    public MapVector(double endX, double endY)
    {
        Start = new Point(0, 0);
        End = new Point(endX, endY);
        lengthCache = null;
        directionCache = null;
    }

    // ReSharper disable InconsistentNaming
    public int VX => End.X - Start.X;
    public int VY => End.Y - Start.Y;
    // ReSharper restore InconsistentNaming

    public double Length
    {
        get
        {
            if (lengthCache != null) return lengthCache.Value;
            lengthCache = Point.GetDistance(Start, End);
            return lengthCache.Value;
        }
    }

    public Direction Direction
    {
        get
        {
            if (directionCache != null) return directionCache.Value;
            directionCache = Direction.GetDirection(Start, End);
            return directionCache.Value;
        }
    }

    public static bool operator ==(MapVector a, MapVector b) => a.Key == b.Key;
    public static bool operator !=(MapVector a, MapVector b) => a.Key != b.Key;
    public override string ToString() => Key;

    public static double GetPointDistance(Point point, MapVector vector, bool absolute = true)
    {
        var start2Point = new MapVector(vector.Start, point);
        var startAngleDiff = Direction.Diff(
            start2Point.Direction,
            vector.Direction,
            absolute);
        var end2Point = new MapVector(vector.End, point);
        var endAngleDiff = Direction.Diff(
            end2Point.Direction,
            vector.Direction,
            absolute);

        if (startAngleDiff.AbsDegree > 90) return start2Point.Length;
        if (endAngleDiff.AbsDegree <= 90) return end2Point.Length;

        var missDistance = start2Point.Length * Math.Sin(Angle.ToRadian(startAngleDiff.Degree));
        return missDistance;
    }
}