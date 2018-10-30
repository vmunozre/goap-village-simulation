using UnityEngine;

public class FarmFarmerAction : GoapAction
{
    private bool farmed = false;
    private OrchardBuilding targetOrchard = null;

    private float startTime = 0;
    private float timeEnergyLoss = 3f;
    private int energyCost = 10;

    public FarmFarmerAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addEffect("harvestTime", true);
    }


    public override void reset()
    {
        farmed = false;
        targetOrchard = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return farmed;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));
        if (farmer.actualOrchard != null)
        {
            targetOrchard = farmer.actualOrchard;
            if (targetOrchard != null)
            {
                float diff = 0.25f;
                float posX = targetOrchard.transform.position.x + Random.Range(-diff, diff);
                float posY = targetOrchard.transform.position.y + Random.Range(-diff, diff);

                targetPosition = new Vector3(posX, posY, agent.transform.position.z);
            }
        }
        else
        {
            OrchardBuilding[] orchards = (OrchardBuilding[])FindObjectsOfType(typeof(OrchardBuilding));
            OrchardBuilding finalOrchard = null;
            if (orchards != null && orchards.Length > 0)
            {
                foreach (OrchardBuilding orchard in orchards)
                {
                    if (!orchard.full && orchard.blueprint.done)                    
                    {
                        finalOrchard = orchard;
                    }
                }
            }

            if (finalOrchard == null)
            {
                addOrchardRequest(farmer);
                return false;
            }

            if (!finalOrchard.addFarmer())
            {
                addOrchardRequest(farmer);
                return false;
            }

            farmer.actualOrchard = finalOrchard;
            targetOrchard = farmer.actualOrchard;

            float diff = 0.25f;
            float posX = targetOrchard.transform.position.x + Random.Range(-diff, diff);
            float posY = targetOrchard.transform.position.y + Random.Range(-diff, diff);

            targetPosition = new Vector3(posX, posY, agent.transform.position.z);
        }

        return targetOrchard != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if ((Time.time - startTime) >= timeEnergyLoss)
        {
            startTime = Time.time;
            Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));
            targetOrchard.farmProgress++;            
            farmer.energy -= energyCost;
            if (farmer.energy <= 0)
            {
                disableBubbleIcon(agent);
                return false;
            } 
        }

        if (targetOrchard.farmProgress > targetOrchard.effort)
        {
            disableBubbleIcon(agent);
            Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));
            farmer.energy -= energyCost;
            if (targetOrchard.food <= 0)
            {
                targetOrchard.farmed();
            }
            farmed = true;
        }
        return true;
    }

    private void addOrchardRequest(Farmer _farmer)
    {       
        Building building = new Building("Prefabs/Buildings/Orchard", 100, 50, 15, 3);
        _farmer.center.addNewBuildingRequest(building);
        Debug.Log(".------------Go to sleeep!");
        _farmer.waiting = true;
    }
}


