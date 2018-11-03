using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : Agent
{
    // Basic data
    public new string name = "Woodcutter";
    public int wood = 0;
    public TreeEntity actualTree = null;

    public SawmillBuilding sawmill = null;
    // Use this for initialization
    void Start()
    {
        center.agentsCounter[name]++;

        SawmillBuilding[] sawmills = (SawmillBuilding[])FindObjectsOfType(typeof(SawmillBuilding));
        foreach (SawmillBuilding saw in sawmills)
        {
            if (!saw.blueprint.done)
            {
                continue;
            }
            sawmill = saw;
            sawmill.workers++;
            break;
            
        }
        if (sawmill == null)
        {
            Building building = new Building("Prefabs/Buildings/Sawmill", 200, 150, 5, 2);
            center.addNewBuildingRequest(building);
        }
    }

    // Update is called once per frame
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

        if (actualTree != null)
        {
            goal.Add("collectWood", true);
        }

        if (actualTree == null)
        {
            goal.Add("treeFound", true);
        }

        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasWood", (wood > 0));
        worldData.Add("hasEnergy", (energy > 0));
        worldData.Add("hasActualTree", (actualTree != null));
        worldData.Add("treeIsChopped", (actualTree != null)&&(actualTree.chopped));

        return worldData;
    }
}
