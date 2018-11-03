using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Agent
{
    // Basic data
    public new string name = "Builder";
    public int stone = 0;
    public int wood = 0;
    public GameObject actualBuilding = null;
    public Building actualRequest = null;

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

        if(actualBuilding == null)
        {
            goal.Add("hasActualBuilding", true);
        }

        if(actualBuilding != null)
        {
            goal.Add("completeRequest", true);            
        }

        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();

        worldData.Add("isWaiting", waiting);
        worldData.Add("hasResources", (stone > 0) || (wood > 0));
        worldData.Add("hasEnergy", (energy > 0));
        worldData.Add("hasActualBuilding", (actualBuilding != null));
        worldData.Add("hasActualRequest", (actualRequest != null));

        if(actualBuilding != null)
        {
            BaseBuilding building = actualBuilding.GetComponent<BaseBuilding>();
            worldData.Add("buildingSupply", building.blueprint.hasAllResources());
            worldData.Add("buildComplete", building.blueprint.done);
        }
        else
        {
            worldData.Add("buildingSupply", false);
            worldData.Add("buildComplete", false);
        }

        return worldData;
    }
}
