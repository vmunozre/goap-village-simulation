using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Agent
{
    // Basic data
    public int stone = 0;
    public int wood = 0;
    public GameObject actualBuilding = null;
    public Building actualRequest = null;
    // Update is called once per frame
    void Update()
    {
        checkIsAdult();
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("hasActualBuilding", true));
        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasResources", (stone > 0) || (wood > 0)));
        //worldData.Add(new KeyValuePair<string, object>("hasWood", (wood > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasActualBuilding", (actualBuilding != null)));
        worldData.Add(new KeyValuePair<string, object>("hasActualRequest", (actualRequest != null)));

        return worldData;
    }
}
