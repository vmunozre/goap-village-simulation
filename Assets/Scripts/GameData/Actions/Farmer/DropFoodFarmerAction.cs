using UnityEngine;

public class DropFoodFarmerAction : GoapAction
{
    private bool droppedFood = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;
    public float dropDuration = 1.5f; // seconds

    public DropFoodFarmerAction()
    {
        addPrecondition("hasFood", true);
        addEffect("hasFood", false);
        addEffect("collectFood", true);
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
        // Debug line
        // Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return targetWarehouse != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > dropDuration)
        {
            Farmer farmer = (Farmer)agent.GetComponent(typeof(Farmer));
            targetWarehouse.food += farmer.food;
            farmer.food = 0;
            droppedFood = true;
        }
        return true;
    }
}

