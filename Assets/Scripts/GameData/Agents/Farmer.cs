﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Agent
{
    // Basic data
    public int food = 0;
    public OrchardBuilding actualOrchard = null;

    // Update is called once per frame
    void Update()
    {
        checkIsAdult();
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        if (waiting)
        {
            goal.Add(new KeyValuePair<string, object>("waitComplete", true));
            return goal;
        }

        goal.Add(new KeyValuePair<string, object>("collectFood", true));
        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("isWaiting", waiting));
        worldData.Add(new KeyValuePair<string, object>("hasFood", (food > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        if(actualOrchard != null)
        {
            worldData.Add(new KeyValuePair<string, object>("harvestTime", (actualOrchard.farmProgress >= actualOrchard.effort)));
        } else
        {
            worldData.Add(new KeyValuePair<string, object>("harvestTime", false));
        }

        return worldData;
    }
}