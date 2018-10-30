using UnityEngine;

public class DropResourcesCarrierAction : GoapAction
{
    private bool dropped = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;
    public float dropDuration = 2.5f; // seconds

    public DropResourcesCarrierAction()
    {
        addPrecondition("hasResources", true);
        addEffect("hasResources", false);
        addEffect("collectResources", true);
    }

    public override void reset()
    {
        dropped = false;
        targetWarehouse = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return dropped;
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
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > dropDuration)
        {
            disableBubbleIcon(agent);
            Carrier carrier = (Carrier)agent.GetComponent(typeof(Carrier));
            targetWarehouse.food += carrier.food;
            targetWarehouse.wood += carrier.wood;
            carrier.food = 0;
            carrier.wood = 0;
            dropped = true;
        }
        return true;
    }
}

