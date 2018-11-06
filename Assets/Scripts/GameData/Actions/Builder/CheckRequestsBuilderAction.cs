using UnityEngine;
public class CheckRequestsBuilderAction : GoapAction
{
    private bool isChecked = false;
    private CenterEntity targetCenter;

    private float startTime = 0;
    public float checkDuration = 0.5f; // seconds

    // Check building request
    public CheckRequestsBuilderAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualRequest", false);
        addPrecondition("hasActualBuilding", false);
        addEffect("hasActualRequest", true);
    }

    public override void reset()
    {
        isChecked = false;
        targetCenter = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return isChecked;
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
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            Building building = targetCenter.getBuildingRequest();
            
            if (building == null)
            {
                disableBubbleIcon(agent);
                builder.waiting = true;
                return false;
            } else
            {
                Debug.Log("Building added to builder");
                builder.actualRequest = building;
            }
            startTime = Time.time;
        }

        if (Time.time - startTime > checkDuration)
        {
            disableBubbleIcon(agent);
            isChecked = true;
        }
        return true;
    }

}


