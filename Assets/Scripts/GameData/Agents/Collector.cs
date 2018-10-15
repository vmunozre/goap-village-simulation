using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : Agent {
    // Basic data
    public int food = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        goal.Add(new KeyValuePair<string, object>("collectFood", true));
        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasFood", (food > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));

        return worldData;
    }
}
