using UnityEngine;

public class CollectFoodCollectorAction : GoapAction
{
    private bool collected = false;
    private BushEntity targetBush = null;

    private float startTime = 0;
    private int energyCost = 30;

    public CollectFoodCollectorAction()
    {
        setBaseDuration(3f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualBush", true);
        addEffect("hasFood", true);
    }

    public override void reset()
    {
        collected = false;
        targetBush = null;
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
        Collector collector = (Collector)agent.GetComponent(typeof(Collector));
        
        targetBush = collector.actualBush;
        if(targetBush != null)
        {
            target = targetBush.gameObject;
        }
        return targetBush != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        // No more food
        if (targetBush.food <= 0)
        {
            disableBubbleIcon(agent);
            Collector collector = (Collector)agent.GetComponent(typeof(Collector));
            targetBush.turnEmptySprite();
            collector.actualBush = null;
            return false;
        }

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Collector collector = (Collector)agent.GetComponent(typeof(Collector));
            int food = 30;
            targetBush.collected = true;
            if ((targetBush.food - food) >= 0)
            {
                collector.food += food;
                targetBush.food -= food;
            }
            else
            {
                collector.food += targetBush.food;
                targetBush.food = 0;
            }
            collector.energy -= energyCost;
            collected = true;
        }
        return true;
    }

}


