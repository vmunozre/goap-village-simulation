using System.Collections.Generic;
using UnityEngine;

public class LakeEntity : MonoBehaviour
{
    public bool full = false;
    // Counters
    public int fishers = 0;
    // Positions list
    public List<GameObject> positionsEmpty;

    // Add fisher
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
