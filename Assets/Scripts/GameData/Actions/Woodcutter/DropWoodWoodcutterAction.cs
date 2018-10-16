using UnityEngine;

public class DropWoodWoodcutterAction : GoapAction
{
    private bool droppedWood = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;
    public float dropDuration = 1.5f; // seconds

    public DropWoodWoodcutterAction()
    {
        addPrecondition("hasWood", true);
        addEffect("hasWood", false);
        addEffect("collectWood", true);
    }

    public override void reset()
    {
        droppedWood = false;
        targetWarehouse = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedWood;
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
            Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));
            targetWarehouse.wood += woodcutter.wood;
            woodcutter.wood = 0;
            droppedWood = true;
        }
        return true;
    }
}

