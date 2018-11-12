using UnityEngine;

public class DropInHuntingShedHunterAction : GoapAction
{
    private bool droppedFood = false;

    private float startTime = 0;

    // Drop food in hunting shed
    public DropInHuntingShedHunterAction()
    {
        setActionName("Drop food in Hunting shed");
        setBaseDuration(1.5f);
        changeDefaultCost(0.5f);
        addPrecondition("hasFood", true);
        addEffect("hasFood", false);
        addEffect("collectFood", true);
    }


    public override void reset()
    {
        droppedFood = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedFood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));

        if (hunter.huntingShed != null)
        {
            target = hunter.huntingShed.gameObject;
        }
        
        // Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return target != null;
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
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            if (hunter.huntingShed != null)
            {
                hunter.huntingShed.food += hunter.food;
            }
            
            hunter.food = 0;
            droppedFood = true;
        }
        return true;
    }
}

