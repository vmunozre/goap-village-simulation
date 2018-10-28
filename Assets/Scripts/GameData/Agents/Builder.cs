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

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        if (waiting)
        {
            goal.Add(new KeyValuePair<string, object>("waitComplete", true));
            return goal;
        }

        if(actualBuilding == null)
        {
            goal.Add(new KeyValuePair<string, object>("hasActualBuilding", true));
        }

        if(actualBuilding != null)
        {
            goal.Add(new KeyValuePair<string, object>("completeRequest", true));            
        }

        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("isWaiting", waiting));
        worldData.Add(new KeyValuePair<string, object>("hasResources", (stone > 0) || (wood > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasActualBuilding", (actualBuilding != null)));
        worldData.Add(new KeyValuePair<string, object>("hasActualRequest", (actualRequest != null)));

        if(actualBuilding != null)
        {
            BaseBuilding building = actualBuilding.GetComponent<BaseBuilding>();
            worldData.Add(new KeyValuePair<string, object>("buildingSupply", building.blueprint.hasAllResources()));
            worldData.Add(new KeyValuePair<string, object>("buildComplete", building.blueprint.done));
        }
        else
        {
            worldData.Add(new KeyValuePair<string, object>("buildingSupply", false));
            worldData.Add(new KeyValuePair<string, object>("buildComplete", false));
        }

        return worldData;
    }
}
