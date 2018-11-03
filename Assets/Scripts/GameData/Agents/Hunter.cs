using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Agent
{
    // Basic data
    public new string name = "Hunter";
    public int food = 0;
    public DeerEntity actualPrey = null;

    // Multiple agents
    public Hunter coopHunter = null;
    public bool leader = false;
    public bool hasTender = false;
    public bool isInPosition = false;
    
    public HuntingShedBuilding huntingShed = null;
    void Start()
    {
        center.agentsCounter[name]++;

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

        if (!hasTender)
        {
            goal.Add("checkTender", true);
            return goal;
        }

        if(actualPrey == null)
        {
            goal.Add("preyFound", true);
            return goal;
        }

        if(actualPrey != null)
        {
            goal.Add("collectFood", true);
            return goal;
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
        worldData.Add("hasTender", hasTender);
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
