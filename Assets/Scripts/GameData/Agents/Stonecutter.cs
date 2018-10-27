using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stonecutter : Agent
{
    // Basic data
    public int stone = 0;
    public QuarryEntity actualQuarry = null;

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

        goal.Add(new KeyValuePair<string, object>("collectStone", true));
        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasStone", (stone > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));

        return worldData;
    }
}
