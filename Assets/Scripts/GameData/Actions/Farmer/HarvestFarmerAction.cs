using UnityEngine;
public class HarvestFarmerAction : GoapAction
{
    private bool collected = false;
    private OrchardBuilding targetOrchard;

    private float startTime = 0;

    public int agentCapacity = 30;
    private int energyCost = 10;

    // Harvest Orchard
    public HarvestFarmerAction()
    {
        setActionName("Harvest orchard");
        setBaseDuration(15f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("harvestTime", true);
        addEffect("hasFood", true);
    }


    public override void reset()
    {
        collected = false;
        targetOrchard = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return collected;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // Move to random position in the orchard
        Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));
        if(farmer.actualOrchard != null)
        {
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
            Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));

            if(farmer.actualOrchard.food <= 0)
            {
                disableBubbleIcon(agent);
                farmer.actualOrchard.toggleSpriteEmpty();
                farmer.actualOrchard.farmProgress = 0f;
                return false;
            }

            startTime = Time.time;
        }
        // Add progress
        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));
            if (farmer.actualOrchard.food >= agentCapacity)
            {
                farmer.actualOrchard.food -= agentCapacity;
                farmer.food += agentCapacity;
            }
            else
            {
                farmer.food = farmer.actualOrchard.food;
                farmer.actualOrchard.food = 0;
            }
            farmer.energy -= energyCost;
            collected = true;
        }
        return true;
    }

}


