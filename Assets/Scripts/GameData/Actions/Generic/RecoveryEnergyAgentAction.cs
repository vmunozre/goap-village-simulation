using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RecoveryEnergyAgentAction : GoapAction
{
    private bool recovered = false;
    private CenterEntity targetCenter;

    private float startTime = 0;
    public float recoveringDuration = 5; // seconds

    private bool procreationControl = false;

    public RecoveryEnergyAgentAction()
    {
        addPrecondition("hasEnergy", false); // we need energy
        addEffect("hasEnergy", true);
    }


    public override void reset()
    {
        recovered = false;
        procreationControl = false;
        targetCenter = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return recovered;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
        CenterEntity closest = null;
        if(centers == null)
        {
            return false;
        }
        if (centers.Length > 0)
        {
            closest = centers[0];
        }

        if (closest == null)
            return false;

        targetCenter = closest;
        target = targetCenter.gameObject;
        // Debug line
        Debug.DrawLine(target.transform.position, agent.transform.position, Color.blue, 3, false);
        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        Agent abstractAgent = (Agent)agent.GetComponent(typeof(Agent));
        if (startTime == 0)
        {
            abstractAgent.recovering = true;
            procreationControl = targetCenter.enterAgentToRecover();
            startTime = Time.time;
        }

        if (Time.time - startTime > recoveringDuration)
        {
            abstractAgent.energy = 100;
            abstractAgent.recovering = false;
            if (!procreationControl)
            {
                targetCenter.exitAgentToRecover();
            }
            recovered = true;
        }
        return true;
    }

}


