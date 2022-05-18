// https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
// This code contributed by PrinciRaj1992

namespace Kicker.Shared;

public static class Geometry
{
    public static bool OnSegment(
        PointS p,
        PointS q,
        PointS r)
        => q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
           q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y);

    public static short GetOrientation(
        PointS p,
        PointS q,
        PointS r)
    {
        var val = (q.Y - p.Y) * (r.X - q.X) -
                  (q.X - p.X) * (r.Y - q.Y);

        if (val == 0)
            return 0;

        return unchecked((short)(val > 0 ? 1 : 2));
    }

    public static bool DoIntersect(
        PointS p1,
        PointS q1,
        PointS p2,
        PointS q2)
    {
        var o1 = GetOrientation(p1, q1, p2);
        var o2 = GetOrientation(p1, q1, q2);
        var o3 = GetOrientation(p2, q2, p1);
        var o4 = GetOrientation(p2, q2, q1);

        if (o1 != o2 && o3 != o4)
            return true;

        if (o1 == 0 && OnSegment(p1, p2, q1))
            return true;

        if (o2 == 0 && OnSegment(p1, q2, q1))
            return true;

        if (o3 == 0 && OnSegment(p2, p1, q2))
            return true;

        if (o4 == 0 && OnSegment(p2, q1, q2))
            return true;

        return false;
    }
}