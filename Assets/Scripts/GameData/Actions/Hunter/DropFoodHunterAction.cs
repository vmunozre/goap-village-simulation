using UnityEngine;

public class DropFoodHunterAction : GoapAction
{
    private bool droppedFood = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;
    public float dropDuration = 1.5f; // seconds

    public DropFoodHunterAction()
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
        WarehouseEntity[] warehouses = (WarehouseEntity[])FindObjectsOfType(typeof(WarehouseEntity));
        WarehouseEntity closest = null;
        if (warehouses == null)
        {
            return false;
        }

        if (warehouses.Length > 0)
        {
            closest = warehouses[0];
        }

        if (closest == null)
            return false;

        targetWarehouse = closest;
        target = targetWarehouse.gameObject;
        // Debug line
        Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > dropDuration)
        {
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            targetWarehouse.food += hunter.food;
            hunter.food = 0;
            droppedFood = true;
        }
        return true;
    }
}

