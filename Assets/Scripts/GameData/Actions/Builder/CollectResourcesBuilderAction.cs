using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectResourcesBuilderAction : GoapAction
{
    private bool collected = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;
    public float checkDuration = 0.5f; // seconds

    public CollectResourcesBuilderAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualRequest", true);
        addPrecondition("hasActualBuilding", true);
        addPrecondition("hasResources", false);       
        addEffect("hasResources", true);
    }


    public override void reset()
    {
        collected = false;
        targetWarehouse = null;
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
        return closest != null;
    }

    public override bool perform(GameObject agent)
    {

        if (startTime == 0)
        {
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();
            
 
            startTime = Time.time;
        }

        if (Time.time - startTime > checkDuration)
        {
            collected = true;
        }
        return true;
    }

}


