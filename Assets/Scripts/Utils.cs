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
    
}