using System.Collections.Generic;

public class Carrier : Agent
{
    public int wood = 0;
    public int food = 0;
    public SawmillBuilding sawmill = null;
    public HuntingShedBuilding huntingShed = null;

    private new string name = "Carrier";

    void Start()
    {
        center.agentsCounter[name]++;

        // Find sawmill or HunyerShed to work
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
            // No building found, turn waiting
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

    public override Dictionary<string, object> createGoalState()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>();
        if (waiting)
        {
            goal.Add("waitComplete", true);
            return goal;
        }

        goal.Add("collectResources", true);

        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasResources", (wood > 0 || food > 0));
        worldData.Add("hasEnergy", (energy > 0));

        return worldData;
    }
}
