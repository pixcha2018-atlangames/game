using UnityEngine;

public struct Bounds2D
{

    public Vector2 center;
    public Vector2 size;

    public Bounds2D(Vector2 center, Vector2 size)
    {
        this.center = center;
        this.size = size;
    }

    public Vector2 min
    {
        get
        {
            return center - extents;
        }
    }

    public Vector2 max
    {
        get
        {
            return center + extents;
        }
    }

    public Vector2 extents
    {
        get
        {
            return size / 2;
        }
    }
    public Bounds2D minkowskiDifference(Bounds2D other)
    {
        Vector2 topLeft = min - other.max;
        Vector2 fullSize = size + other.size;
        return new Bounds2D(topLeft + (fullSize / 2), fullSize);
    }

    public static Bounds2D minkowskiDifference(Bounds2D a, Bounds2D b)
    {
        Vector2 topLeft = a.min - b.max;
        Vector2 fullSize = a.size + b.size;
        return new Bounds2D(topLeft + (fullSize / 2), fullSize);
    }

    public static Bounds2D boundsXZTo2D(Bounds bounds)
    {
        return new Bounds2D(
            new Vector2(bounds.center.x, bounds.center.z),
            new Vector2(bounds.size.x, bounds.size.z)
        );
    }

    public static Bounds2D boundsXYTo2D(Bounds bounds)
    {
        return new Bounds2D(
            new Vector2(bounds.center.x, bounds.center.y),
            new Vector2(bounds.size.x, bounds.size.y)
        );
    }

    public Vector2 closestPointOnBoundsToPoint(Vector2 point)
    {
        float minDist = Mathf.Abs(point.x - min.x);
        Vector2 boundsPoint = new Vector2(min.x, point.y);
        if (Mathf.Abs(max.x - point.x) < minDist)
        {
            minDist = Mathf.Abs(max.x - point.x);
            boundsPoint = new Vector2(max.x, point.y);
        }
        if (Mathf.Abs(max.y - point.y) < minDist)
        {
            minDist = Mathf.Abs(max.y - point.y);
            boundsPoint = new Vector2(point.x, max.y);
        }
        if (Mathf.Abs(min.y - point.y) < minDist)
        {
            minDist = Mathf.Abs(min.y - point.y);
            boundsPoint = new Vector2(point.x, min.y);
        }
        return boundsPoint;
    }

    public float intersectionWithRay2D_Old(Ray2D ray)
    {
        float dims = 2;

        float lo = float.NegativeInfinity;
        float hi = float.PositiveInfinity;

        for (var i = 0; i < dims; i++)
        {
            float dimLo = (this.center[i] - ray.origin[i]) / ray.direction[i];
            float dimHi = (this.size[i] - ray.origin[i]) / ray.direction[i];

            if (dimLo > dimHi)
            {
                float tmp = dimLo;
                dimLo = dimHi;
                dimHi = tmp;
            }

            if (dimHi < lo || dimLo > hi)
            {
                return float.PositiveInfinity;
            }

            if (dimLo > lo) lo = dimLo;
            if (dimHi < hi) hi = dimHi;
        }

        return lo > hi ? float.PositiveInfinity : lo;
    }

    public Segment2D top {
        get {
            return new Segment2D(
                new Vector2(center.x-extents.x,center.y+extents.y),
                new Vector2(center.x+extents.x,center.y+extents.y)
            );
        }
    }

    public Segment2D bottom {
        get {
            return new Segment2D(
                new Vector2(center.x-extents.x,center.y-extents.y),
                new Vector2(center.x+extents.x,center.y-extents.y)
            );
        }
    }

    public Segment2D left {
        get {
            return new Segment2D(
                new Vector2(center.x-extents.x,center.y-extents.y),
                new Vector2(center.x-extents.x,center.y+extents.y)
            );
        }
    }

    public Segment2D right {
        get {
            return new Segment2D(
                new Vector2(center.x+extents.x,center.y-extents.y),
                new Vector2(center.x+extents.x,center.y+extents.y)
            );
        }
    }

    
    public bool intersectionWithRay2D(Ray2D ray,out Vector2 ret)
    {
        if(Utils.Ray2DToSegment(ray,top,out ret)) return true;
        if(Utils.Ray2DToSegment(ray,right,out ret)) return true;
        if(Utils.Ray2DToSegment(ray,bottom,out ret)) return true;
        if(Utils.Ray2DToSegment(ray,left,out ret)) return true;
        return false;
    }

}