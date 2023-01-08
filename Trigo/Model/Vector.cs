using System;

namespace Syntactic.Sugar.Trigo.Model;

public struct Vector
{
    public readonly double VX;
    public readonly double VY;
    private double? lengthCache;
    private Direction? directionCache;
    public string Key => $"->{VX:F3},{VY:F3}";

    public Vector(Point relTarget)
    {
        VX = relTarget.X;
        VY = relTarget.Y;
        lengthCache = null;
        directionCache = null;
    }

    public Vector(Point start, Point end)
    {
        VX = end.X - start.X;
        VY = end.Y - start.Y;
        lengthCache = null;
        directionCache = null;
    }

    public Vector(int endX, int endY)
    {
        VX = endX;
        VY = endY;
        lengthCache = null;
        directionCache = null;
    }

    public Vector(double endX, double endY)
    {
        VX = endX;
        VY = endY;
        lengthCache = null;
        directionCache = null;
    }

    public Vector(Vector vector, double length)
    {
        var rate = length / vector.Length;
        VX = vector.VX * rate;
        VY = vector.VY * rate;
        lengthCache = null;
        directionCache = null;
    }

    public Point AsPoint() => new Point(VX, VY);

    public double Length
    {
        get
        {
            if (lengthCache != null) return lengthCache.Value;
            lengthCache = Point.GetDistance(new Point(0, 0), AsPoint());
            return lengthCache.Value;
        }
    }

    public Direction Direction
    {
        get
        {
            if (directionCache != null) return directionCache.Value;
            directionCache = Direction.GetDirection(new Point(0, 0), AsPoint());
            return directionCache.Value;
        }
    }

    public static bool operator ==(Vector a, Vector b) => a.Key == b.Key;
    public static bool operator !=(Vector a, Vector b) => a.Key != b.Key;
    public static Vector operator +(Vector a, Vector b) => new Vector(a.VX + b.VX, a.VY + b.VY);
    public static Vector operator -(Vector a, Vector b) => new Vector(a.VX - b.VX, a.VY - b.VY);
    public static Vector operator *(Vector v, double d) => new Vector(v.VX * d, v.VY * d);
    public static Vector operator /(Vector v, double d) => new Vector(v.VX / d, v.VY / d);
    public override string ToString() => Key;

    // TODO tests
    public static double DotProduct(Vector a, Vector b)
    {
        var diffAngle = Direction.Diff(a.Direction, b.Direction);
        return a.Length * b.Length * Math.Cos(Angle.ToRadian(diffAngle.Degree));
    }
}