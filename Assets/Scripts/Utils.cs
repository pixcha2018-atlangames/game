using UnityEngine;

public class Utils {
    public static float GetAverage(float[] value)
    {
        float sum = 0;

        int size = value.Length;
        for(int i=0;i<size;i++){
            sum+=value[i];
        }

        return sum/size;
        
    }

    public static T[] shuffle<T>(T[] array)
    {
        
        int size = array.Length;
        T[] ret = new T[size];
        array.CopyTo(ret,0);
        for (int i = 0; i < size; i++ )
        {
            T tmp = ret[i];
            int r = Random.Range(i, array.Length);
            ret[i] = ret[r];
            ret[r] = tmp;
        }

        return ret;
    }

    public static Bounds CreateBounds(Vector3[] gos) {
        if (gos.Length == 0)
            return new Bounds(Vector3.zero,Vector3.zero);
        if (gos.Length == 1)
            return new Bounds(gos[0], Vector3.zero);
        Bounds bounds = new Bounds(gos[0],Vector3.zero);
        for (var i = 1; i < gos.Length; i++)
            bounds.Encapsulate(gos[i]); 
        return bounds;
    }

    public static bool Ray2DToSegment(Ray2D ray, Segment2D seg, out Vector2 ret)
    {
        float r, s, d;
        float dx = ray.direction.x;
        float dy = ray.direction.y;
        float x = ray.origin.x;
        float y = ray.origin.y;
        float x1 = seg.a.x;
        float x2 = seg.b.x;
        float y1 = seg.a.y;
        float y2 = seg.b.y;
        //Make sure the lines aren't parallel, can use an epsilon here instead
        // Division by zero in C# at run-time is infinity. In JS it's NaN
        if (dy / dx != (y2 - y1) / (x2 - x1))
        {
            d = ((dx * (y2 - y1)) - dy * (x2 - x1));
            if (d != 0)
            {
                r = (((y - y1) * (x2 - x1)) - (x - x1) * (y2 - y1)) / d;
                s = (((y - y1) * dx) - (x - x1) * dy) / d;
                if (r >= 0 && s >= 0 && s <= 1)
                {
                    ret = new Vector2(
                        x + r * dx,
                        y + r * dy
                    );
                    return true;
                }
            }
        }
        ret = Vector2.zero;
        return false;
    }

}