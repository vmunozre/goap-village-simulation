using System.Collections.Generic;
using UnityEngine;

public class LakeEntity : MonoBehaviour
{
    public bool full = false;
    public int fishers = 0;
    public List<GameObject> positionsEmpty;

    void Start()
    {
        //Por ahora limite fijado en 4
        //limitWorkers = Random.Range(3, 7);
    }

    public GameObject addFisher()
    {
        GameObject position = null;
        if(positionsEmpty.Count > 0)
        {
            fishers++;
            position = positionsEmpty[0];
            positionsEmpty.RemoveAt(0);
        } else
        {
            full = true;
        }
        return position;
    }
}
