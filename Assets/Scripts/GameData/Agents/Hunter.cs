using System.Collections.Generic;

public class Hunter : Agent
{    
    public int food = 0;
    public DeerEntity actualPrey = null;

    // Multiple agents
    public Hunter coopHunter = null;
    public HuntingShedBuilding huntingShed = null;
    public TenderRequest tenderRequest;
    public bool leader = false;
    public bool isInPosition = false;

    private new string name = "Hunter";
    void Start()
    {
        center.agentsCounter[name]++;

        // Find HuntingShed
        HuntingShedBuilding[] huntingSheds = (HuntingShedBuilding[])FindObjectsOfType(typeof(HuntingShedBuilding));
        foreach (HuntingShedBuilding shed in huntingSheds)
        {
            if (!shed.blueprint.done)
            {
                continue;
            }
            huntingShed = shed;
            huntingShed.hunters++;
            break;

        }
        // If not found send building request
        if (huntingShed == null)
        {
            Building building = new Building("Prefabs/Buildings/huntingShed", 250, 150, 7, 2);
            center.addNewBuildingRequest(building);
        }
        moveSpeed = 1;
    }

    public override Dictionary<string, object> createGoalState()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>();
        if (waiting)
        {
            goal.Add("waitComplete", true);
            return goal;
        }

        if (tenderRequest == null)
        {
            goal.Add("checkTender", true);
            return goal;
        }

        if(actualPrey == null)
        {
            goal.Add("preyFound", true);
           
        } else
        {
            goal.Add("collectFood", true);
        }
        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasFood", (food > 0));
        worldData.Add("hasEnergy", (energy > 0));
        worldData.Add("hasActualPrey", (actualPrey != null));
        worldData.Add("hasTender", tenderRequest != null);
        worldData.Add("hasCoopHunter", coopHunter != null);

        if(coopHunter != null)
        {
            worldData.Add("isInPosition", isInPosition && coopHunter.isInPosition);
        } else
        {
            worldData.Add("isInPosition", false);
        }
        
        if (actualPrey != null)
        {
            worldData.Add("hasDeadPrey", actualPrey.isDead);
        } else
        {
            worldData.Add("hasDeadPrey", false);
        }
        return worldData;
    }

}
