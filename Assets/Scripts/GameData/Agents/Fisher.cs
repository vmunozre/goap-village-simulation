using System.Collections.Generic;
using UnityEngine;

public class Fisher : Agent
{
    public int food = 0;
    public GameObject actualLakePosition = null;

    private new string name = "Fisher";

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

        goal.Add("collectFish", true);
        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasFood", (food > 0));
        worldData.Add("hasEnergy", (energy > 0));

        return worldData;
    }
}
