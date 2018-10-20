using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{
    // Basic data
    public int food = 0;
    public DeerEntity actualPrey = null;

    private void Start()
    {
        moveSpeed = 1;
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        if(actualPrey == null)
        {
            goal.Add(new KeyValuePair<string, object>("preyFound", true));        
        }

        if(actualPrey != null)
        {
            goal.Add(new KeyValuePair<string, object>("collectFood", true));        
        }
        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasFood", (food > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasActualPrey", (actualPrey != null)));

        if (actualPrey != null)
        {
            worldData.Add(new KeyValuePair<string, object>("hasDeadPrey", actualPrey.isDead));
        } else
        {
            worldData.Add(new KeyValuePair<string, object>("hasDeadPrey", false));
        }
        return worldData;
    }
}
