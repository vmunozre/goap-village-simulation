using UnityEngine;

public class CollectFoodHunterAction : GoapAction
{
    private bool collected = false;
    private DeerEntity targetPrey = null;

    private float startTime = 0; 
    private int energyCost = 30;

    // Collect food from prey
    public CollectFoodHunterAction()
    {
        setActionName("Collect food");
        setBaseDuration(3f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualPrey", true);
        addPrecondition("hasDeadPrey", true);
        addEffect("hasFood", true);
    }

    public override void reset()
    {
        collected = false;
        targetPrey = null;
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
        }

        // Prey empty
        if (targetPrey.food <= 0)
        {
            disableBubbleIcon(agent);
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            hunter.actualPrey.turnEmpty();
            // Remove tender
            hunter.center.removeTenderList(hunter.tenderRequest);
            hunter.tenderRequest = null;
            hunter.actualPrey = null;           
            return false;
        }

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            
            int food = Random.Range(20, 40);
           
            if ((targetPrey.food - food) >= 0)
            {
                hunter.food += food;
                targetPrey.food -= food;
            }
            else
            {
                hunter.food += targetPrey.food;
                targetPrey.food = 0;
            }
            hunter.energy -= energyCost;
            collected = true;
        }
        return true;
    }

}


