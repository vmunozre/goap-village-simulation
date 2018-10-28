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

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        if (waiting)
        {
            goal.Add(new KeyValuePair<string, object>("waitComplete", true));
            return goal;
        }

        if (actualTree != null)
        {
            goal.Add(new KeyValuePair<string, object>("collectWood", true));
        }

        if (actualTree == null)
        {
            goal.Add(new KeyValuePair<string, object>("treeFound", true));
        }

        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("isWaiting", waiting));
        worldData.Add(new KeyValuePair<string, object>("hasWood", (wood > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasActualTree", (actualTree != null)));
        worldData.Add(new KeyValuePair<string, object>("treeIsChopped", (actualTree != null)&&(actualTree.chopped)));

        return worldData;
    }
}
