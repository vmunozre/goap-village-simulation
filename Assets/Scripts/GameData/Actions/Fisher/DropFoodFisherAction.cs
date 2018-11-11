using UnityEngine;

public class DropFoodFisherAction : GoapAction
{
    private bool droppedFood = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;

    // Drop food
    public DropFoodFisherAction()
    {
        setActionName("Drop fish");
        setBaseDuration(1.5f);
        addPrecondition("hasFood", true);
        addEffect("hasFood", false);
        addEffect("collectFish", true);
    }

    public override void reset()
    {
        droppedFood = false;
        targetWarehouse = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedFood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Agent abstractAgent = (Agent)agent.GetComponent(typeof(Agent));
        targetWarehouse = abstractAgent.warehouse;
        target = targetWarehouse.gameObject;
        // Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return targetWarehouse != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Fisher fisher = (Fisher)agent.GetComponent(typeof(Fisher));
            targetWarehouse.food += fisher.food;
            fisher.food = 0;
            droppedFood = true;
        }
        return true;
    }
}

