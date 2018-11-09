using System.Collections.Generic;

public class Farmer : Agent
{    
    public int food = 0;
    public OrchardBuilding actualOrchard = null;

    private new string name = "Farmer";

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

        goal.Add("collectFood", true);
        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasFood", (food > 0));
        worldData.Add("hasEnergy", (energy > 0));
        if(actualOrchard != null)
        {
            worldData.Add("harvestTime", (actualOrchard.farmProgress >= actualOrchard.effort));
        } else
        {
            worldData.Add("harvestTime", false);
        }

        return worldData;
    }
}
