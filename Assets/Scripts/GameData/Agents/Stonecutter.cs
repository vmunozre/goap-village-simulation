﻿using System.Collections.Generic;

public class Stonecutter : Agent
{
    public int stone = 0;
    public QuarryEntity actualQuarry = null;

    private new string name = "Stonecutter";
    void Start()
    {
        center.agentsCounter[name]++;
    }

    void Update()
    {
        checkSuperUpdate();
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
