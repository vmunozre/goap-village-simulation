using System.Collections.Generic;

public class Woodcutter : Agent
{
    public int wood = 0;
    public TreeEntity actualTree = null;
    public SawmillBuilding sawmill = null;

    private new string name = "Woodcutter";

    void Start()
    {
        center.agentsCounter[name]++;
        // Find sawmill building
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
        // If not found send building request
        if (sawmill == null)
        {
            Building building = new Building("Prefabs/Buildings/Sawmill", 200, 150, 5, 2);
            center.addNewBuildingRequest(building);
        }
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

        if (actualTree != null)
        {
            goal.Add("collectWood", true);
        } else
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
