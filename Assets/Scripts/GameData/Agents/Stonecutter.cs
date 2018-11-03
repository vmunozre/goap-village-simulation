using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stonecutter : Agent
{
    // Basic data
    public new string name = "Stonecutter";
    public int stone = 0;
    public QuarryEntity actualQuarry = null;

    void Start()
    {
        center.agentsCounter[name]++;
    }

    void Update()
    {
        checkIsAdult();
    }

    public override Dictionary<string, object> createGoalState()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>();
        if (waiting)
        {
            goal.Add("waitComplete", true);
            return goal;
        }

        goal.Add("collectStone", true);
        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasStone", (stone > 0));
        worldData.Add("hasEnergy", (energy > 0));

        return worldData;
    }
}
