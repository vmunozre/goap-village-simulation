﻿using UnityEngine;

public class HuntCoopHunterAction : GoapAction
{
    private bool hunt = false;
    private DeerEntity targetPrey = null;

    private int energyCost = 50;
    private float startTime = 0;

    // Hunt coop
    public HuntCoopHunterAction()
    {
        setActionName("Cooperative hunt");
        setBaseDuration(2.5f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualPrey", true);
        addPrecondition("hasDeadPrey", false);
        addPrecondition("hasCoopHunter", true);
        addPrecondition("isInPosition", true);
        addEffect("hasDeadPrey", true);
    }
    public override void reset()
    {
        hunt = false;
        targetPrey = null;
        startTime = 0;

    }

    public override bool isDone()
    {
        return hunt;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));

        targetPrey = hunter.actualPrey;
        if (targetPrey != null)
        {
            target = targetPrey.gameObject;
        }
        return targetPrey != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            if (!hunter.actualPrey.isDead)
            {
                hunter.actualPrey.killDeer();
            }
        }

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            hunter.energy -= energyCost;
            hunt = true;
        }

        return true;
    }

}


