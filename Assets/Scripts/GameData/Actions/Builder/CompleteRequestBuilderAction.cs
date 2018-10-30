using UnityEngine;
public class CompleteRequestBuilderAction : GoapAction
{
    private bool isCompleted = false;
    private CenterEntity targetCenter;

    private float startTime = 0;
    public float checkDuration = 0.5f; // seconds

    public CompleteRequestBuilderAction()
    {
        addPrecondition("hasActualRequest", true);
        addPrecondition("hasActualBuilding", true);
        addPrecondition("buildComplete", true);
        addEffect("completeRequest", true);
    }


    public override void reset()
    {
        isCompleted = false;
        targetCenter = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return isCompleted;
    }

    public override bool requiresInRange()
    {
        return false;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Agent abstractAgent = (Agent)agent.GetComponent(typeof(Agent));
        targetCenter = abstractAgent.center;
        target = targetCenter.gameObject;
        return targetCenter != null;
    }

    public override bool perform(GameObject agent)
    {

        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > checkDuration)
        {
            disableBubbleIcon(agent);
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));

            if (builder.actualRequest != null)
            {
                targetCenter.removeBuildingRequest(builder.actualRequest);
            }
            builder.actualRequest = null;
            builder.actualBuilding = null;
            isCompleted = true;
        }
        return true;
    }

}


