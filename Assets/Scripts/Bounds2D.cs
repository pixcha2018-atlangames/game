using UnityEngine;

public struct Bounds2D{

    public Vector2 center;
    public Vector2 size;

    public Bounds2D(Vector2 center, Vector2 size){
        this.center = center;
        this.size = size;
    }

    public Vector2 min {
        get{
            return center - extents;
        }
    }

    public Vector2 max {
        get{
            return center + extents;
        }
    }

    public Vector2 extents{
        get {
            return size / 2;
        }
    }
     public Bounds2D minkowskiDifference(Bounds2D other)
    {
        Vector2 topLeft = min - other.max;
        Vector2 fullSize = size + other.size;
        return new Bounds2D(topLeft + (fullSize / 2 ), fullSize);
    }

    public static Bounds2D minkowskiDifference(Bounds2D a, Bounds2D b)
    {
        Vector2 topLeft = a.min - b.max;
        Vector2 fullSize = a.size + b.size;
        return new Bounds2D(topLeft + (fullSize / 2), fullSize);
    }

    public static Bounds2D boundsXZTo2D(Bounds bounds){
        return new Bounds2D(
            new Vector2(bounds.center.x,bounds.center.z),
            new Vector2(bounds.size.x,bounds.size.z)
        );
    }

    public static Bounds2D boundsXYTo2D(Bounds bounds){
        return new Bounds2D(
            new Vector2(bounds.center.x,bounds.center.y),
            new Vector2(bounds.size.x,bounds.size.y)
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


}