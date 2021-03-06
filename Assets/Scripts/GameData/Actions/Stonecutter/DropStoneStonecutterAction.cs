﻿using UnityEngine;

public class DropStoneStonecutterAction : GoapAction
{
    private bool droppedStone = false;
    private WarehouseEntity targetWarehouse;

    private float startTime = 0;

    // Drop stone
    public DropStoneStonecutterAction()
    { 
        setBaseDuration(1.5f);
        setActionName("Drop stone");
        addPrecondition("hasStone", true);
        addEffect("hasStone", false);
        addEffect("collectStone", true);
    }

    public override void reset()
    {
        droppedStone = false;
        targetWarehouse = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedStone;
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
        return abstractAgent != null;
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
            Stonecutter stonecutter = (Stonecutter)agent.GetComponent(typeof(Stonecutter));
            targetWarehouse.stone += stonecutter.stone;
            stonecutter.stone = 0;
            droppedStone = true;
        }
        return true;
    }
}

