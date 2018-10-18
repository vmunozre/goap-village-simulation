using UnityEngine;

public class QuarryEntity : MonoBehaviour
{
    public bool full = false;
    private int limitWorkers = 4;
    public int stonecutters = 0;

    void Start()
    {
        //Por ahora limite fijado en 4
        //limitWorkers = Random.Range(3, 7);
    }

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
