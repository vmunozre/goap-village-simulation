using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaitAgentAction : GoapAction
{
    private bool waited = false;
    private CenterEntity targetCenter;

    private float startTime = 0;
    public float waitDuration = 5; // seconds

    public WaitAgentAction()
    {
        addPrecondition("isWaiting", true); // we need energy
        addEffect("isWaiting", false);
        addEffect("waitComplete", true);
    }


    public override void reset()
    {
        waited = false;
        targetCenter = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return waited;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Agent abstractAgent = (Agent) agent.GetComponent(typeof(Agent));

        targetCenter = abstractAgent.center;
        float diff = 0.5f;
        float posX = targetCenter.transform.position.x + Random.Range(-diff, diff);
        float posY = targetCenter.transform.position.y + Random.Range(-diff, diff);

        targetPosition = new Vector3(posX, posY, agent.transform.position.z);
        // Debug line
        Debug.DrawLine(targetPosition, agent.transform.position, Color.white, 3, false);
        return targetCenter != null;
    }

    public override bool perform(GameObject agent)
    {
        
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > waitDuration)
        {
            disableBubbleIcon(agent);
            Agent abstractAgent = (Agent)agent.GetComponent(typeof(Agent));
            abstractAgent.energy += 5;
            abstractAgent.waiting = false;
            waited = true;
        }
        return true;
    }

}


