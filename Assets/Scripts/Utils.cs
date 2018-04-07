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
        var bounds = new Bounds();
        for (var i = 0; i < gos.Length; i++)
            bounds.Encapsulate(gos[i]); 
        return bounds;
    }

    public static Bounds minkowskiDifference(Bounds a, Bounds b)
    {
        Vector3 topLeft = a.min - b.max;
        Vector3 fullSize = a.size + b.size;
        return new Bounds(topLeft + (fullSize / 2), fullSize / 2);
    }

}