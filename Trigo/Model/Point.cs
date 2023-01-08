using System;

namespace Syntactic.Sugar.Trigo.Model;

public struct Point
{
    public readonly int X;
    public readonly int Y;
    public string Key => $"{X},{Y}";

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Point(double x, double y)
    {
        X = (int) Math.Round(x);
        Y = (int) Math.Round(y);
    }

    public Point(Point orig)
    {
        X = orig.X;
        Y = orig.Y;
    }

    public static bool operator ==(Point a, Point b) => a.Key == b.Key;
    public static bool operator !=(Point a, Point b) => a.Key != b.Key;
    public static Point operator +(Point p, Point b) => new Point(p.X + b.X, p.Y + b.Y);
    public static Point operator -(Point p, Point b) => new Point(p.X - b.X, p.Y - b.Y);
    public static Point operator +(Point p, Vector v) => new Point(p.X + v.VX, p.Y + v.VY);
    public static Point operator -(Point p, Vector v) => new Point(p.X - v.VX, p.Y - v.VY);
    public override string ToString() => Key;

    public static double GetDistance(Point a, Point b)
    {
        var diffX = Math.Abs(a.X - b.X);
        var diffY = Math.Abs(a.Y - b.Y);
        var sqrt = Math.Sqrt(diffX * diffX + diffY * diffY);
        return double.IsNaN(sqrt) ? double.MaxValue : sqrt;
    }
}