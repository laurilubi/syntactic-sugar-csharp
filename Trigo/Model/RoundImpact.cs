using System;

namespace Syntactic.Sugar.Trigo.Model;

// TODO better name
/// <summary>
/// Round object with radius and speed vector.
/// </summary>
public class RoundImpact
{
    public readonly Point Pos;
    public readonly Vector SpeedVector;
    public readonly int Radius;
    public string Key => $"{Pos}-{SpeedVector}-{Radius}";

    public RoundImpact(Point pos, Vector speedVector, int radius)
    {
        Pos = pos;
        SpeedVector = speedVector;
        Radius = radius;
    }

    // public static bool operator ==(Energy a, Energy b) => a?.Key == b?.Key;
    // public static bool operator !=(Energy a, Energy b) => a?.Key != b?.Key;
    public override string ToString() => Key;

    public static (RoundImpact, RoundImpact) GetCollision(RoundImpact a, RoundImpact b)
    {
        var share = GetCollisionShare(a, b);
        if (share == null) return (null, null);

        return (new RoundImpact(a.Pos, a.SpeedVector * share.Value, a.Radius),
            new RoundImpact(b.Pos, b.SpeedVector * share.Value, b.Radius));
    }

    /// <returns>Share (like percentage but 0..1) of A speedvector length at collision point, or null if no collision</returns>
    private static double? GetCollisionShare(RoundImpact a, RoundImpact b)
    {
        var aTmp = new RoundImpact(a.Pos, a.SpeedVector - b.SpeedVector, a.Radius);
        var bTmp = new RoundImpact(b.Pos, new Vector(0, 0), b.Radius);

        return GetStillCollisionShare(aTmp, bTmp);
    }

    /// <param name="a">Moving circle</param>
    /// <param name="b">Still circle, assumes that b has no speedvector.</param>
    /// <returns>Share (like percentage but 0..1) of A speedvector length at collision point, or null if no collision</returns>
    private static double? GetStillCollisionShare(RoundImpact a, RoundImpact b)
    {
        if (b.SpeedVector.VX != 0 || b.SpeedVector.VY != 0) throw new ArgumentException();

        // a.SpeedVector = V
        var radiusSum = a.Radius + b.Radius;
        var abVec = new Vector(a.Pos, b.Pos); // C
        if (a.SpeedVector.Length + radiusSum < abVec.Length) return null;

        var abDotpSpeed = Vector.DotProduct(a.SpeedVector, abVec);
        if (abDotpSpeed < 0) return null;

        var speedVecNorm = new Vector(a.SpeedVector, 1); // N
        var abDotpCp = Vector.DotProduct(speedVecNorm, abVec); // D

        var bCpLenSq = abVec.Length * abVec.Length - abDotpCp * abDotpCp; // F
        if (bCpLenSq > radiusSum * radiusSum) return null;

        var collCpLenSq = radiusSum * radiusSum - bCpLenSq; // T
        var aCollLen = abDotpCp - Math.Sqrt(collCpLenSq);

        if (a.SpeedVector.Length < aCollLen) return null;

        var share = aCollLen / a.SpeedVector.Length;
        return share;

        // var aMapVector = new MapVector(a.Pos, a.SpeedVector);
        // var closestCenterDistance = MapVector.GetPointDistance(b.Pos, aMapVector);
        // if (closestCenterDistance > radiusSum) return null;
        //
        // var closestDistanceVectorLength = Math.Sqrt(
        //     abVec.Length * abVec.Length
        //     - closestCenterDistance * closestCenterDistance); // can be longer than speedvector 
        //
        // var collisionPre = Math.Sqrt(radiusSum * radiusSum - closestCenterDistance * closestCenterDistance);
        // if (collisionPre > closestDistanceVectorLength) throw new Exception();
        //
        // var travelDistance = closestDistanceVectorLength - collisionPre;
        // if (travelDistance > aMapVector.Length) throw new Exception();
    }

    public static (RoundImpact aAfter, RoundImpact bAfter) ApplyCollision(RoundImpact a, RoundImpact b)
    {
        var share = GetCollisionShare(a, b);
        if (share == null)
        {
            // no collision
            var aMoved = new RoundImpact(new Point(a.Pos + a.SpeedVector), a.SpeedVector, a.Radius);
            var bMoved = new RoundImpact(new Point(b.Pos + b.SpeedVector), b.SpeedVector, b.Radius);
            return (aMoved, bMoved);
        }

        var aCollPos = new Point(a.Pos + a.SpeedVector * share.Value);
        var bCollPos = new Point(b.Pos + b.SpeedVector * share.Value);

        var abVec = new Vector(aCollPos, bCollPos);
        var abVecNorm = new Vector(abVec, 1); // n

        var aSpeedDotpAb = Vector.DotProduct(a.SpeedVector, abVecNorm); // a1
        var bSpeedDotpAb = Vector.DotProduct(b.SpeedVector, abVecNorm); // a2

        var optimizedP = aSpeedDotpAb - bSpeedDotpAb;
        // var optimizedP = (2.0 * (aSpeedDotpAb - bSpeedDotpAb)) / (circle1.mass + circle2.mass);

        var aSpeedVectorFinal = a.SpeedVector - abVecNorm * optimizedP;
        var bSpeedVectorFinal = b.SpeedVector + abVecNorm * optimizedP;
        // var aSpeedVectorFinal = a.SpeedVector - abVecNorm * optimizedP * aMass;

        var shareLeft = 1 - share.Value;
        var aAfter = new RoundImpact(aCollPos + aSpeedVectorFinal * shareLeft, aSpeedVectorFinal, a.Radius);
        var bAfter = new RoundImpact(bCollPos + bSpeedVectorFinal * shareLeft, bSpeedVectorFinal, b.Radius);

        return (aAfter, bAfter);
    }
}