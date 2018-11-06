using UnityEngine;

public class QuarryEntity : MonoBehaviour
{
    public bool full = false;
    // Workers counters
    private int limitWorkers = 4;
    public int stonecutters = 0;

    // Add worker
    public bool addStoneCutters()
    {
        if(stonecutters < limitWorkers)
        {
            stonecutters++;
            full = (stonecutters >= limitWorkers);
            return true;
        }
        return false;
    }
}
