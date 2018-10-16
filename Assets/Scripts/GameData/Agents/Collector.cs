using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : Agent {
    // Basic data
    public int food = 0;
    public BushEntity actualBush = null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        checkIsAdult();
	}

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        if (actualBush != null)
        {
            goal.Add(new KeyValuePair<string, object>("collectFood", true));
        }

        if (actualBush == null)
        {
            goal.Add(new KeyValuePair<string, object>("bushFound", true));
        }
        
        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasFood", (food > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasActualBush", (actualBush != null)));

        return worldData;
    }
}
