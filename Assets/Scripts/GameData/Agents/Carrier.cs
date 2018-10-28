using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : Agent
{
    // Basic data
    public new string name = "Carrier";
    public int wood = 0;
    public int food = 0;

    public SawmillBuilding sawmill = null;
    public HuntingShedBuilding huntingShed = null;
    // Use this for initialization
    void Start()
    {
        center.agentsCounter[name]++;

        SawmillBuilding[] sawmills = (SawmillBuilding[])FindObjectsOfType(typeof(SawmillBuilding));
        foreach (SawmillBuilding saw in sawmills)
        {
            if (!saw.blueprint.done || saw.carriers >= saw.limitCarriers)
            {
                continue;
            }
            sawmill = saw;
            sawmill.carriers++;
            break;
        }
        if (sawmill == null)
        {
            HuntingShedBuilding[] huntingSheds = (HuntingShedBuilding[])FindObjectsOfType(typeof(HuntingShedBuilding));
            foreach (HuntingShedBuilding shed in huntingSheds)
            {
                if (!shed.blueprint.done || shed.carriers >= shed.limitCarriers)
                {
                    continue;
                }
                huntingShed = shed;
                huntingShed.carriers++;
                break;
            }
            if(huntingShed == null)
            {
                waiting = true;
            }
        }
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

        goal.Add(new KeyValuePair<string, object>("collectResources", true));

        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("isWaiting", waiting));
        worldData.Add(new KeyValuePair<string, object>("hasResources", (wood > 0 || food > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));

        return worldData;
    }
}
