﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectResourcesBuilderAction : GoapAction
{
    private bool collected = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;
    public float checkDuration = 1.5f; // seconds

    public int agentCapacity = 50;
    private int energyCost = 10;
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
            //TODO - Si no quedan recursos en almacen esperar un tiempo
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();

            int lackWood = building.blueprint.woodCost - building.blueprint.actualWood;
            int lackStone = building.blueprint.stoneCost - building.blueprint.actualStone;

            if(targetWarehouse.wood <= 0 && lackWood > 0)
            {
                builder.waiting = true;
                return false;
            }

            if(targetWarehouse.stone <= 0 && lackStone > 0)
            {
                builder.waiting = true;
                return false;
            }

            if(targetWarehouse.wood >= lackWood)
            {
                builder.wood = Mathf.Min(lackWood, agentCapacity);
                targetWarehouse.wood -= builder.wood;
            } else
            {
                builder.wood = targetWarehouse.wood;
                targetWarehouse.wood -= builder.wood;
            }

            if(builder.wood + builder.stone < agentCapacity)
            {
                if (targetWarehouse.stone >= lackStone)
                {
                    builder.stone = Mathf.Min(lackStone, agentCapacity);
                    targetWarehouse.stone -= builder.stone;
                }
                else
                {
                    builder.stone = targetWarehouse.stone;
                    targetWarehouse.stone -= builder.stone;
                }
            }

            startTime = Time.time;
        }

        if (Time.time - startTime > checkDuration)
        {
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            builder.energy -= energyCost;
            collected = true;
        }
        return true;
    }

}

