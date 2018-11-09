using UnityEngine;

public class AnalyzeBushCollectorAction : GoapAction
{
    private bool analyzed = false;
    public BushEntity targetBush;

    private float startTime = 0;
    private int energyCost = 5;

    // Find 
    private float radius = 1f;
    private int numTry = 1;

    // Find adult bush
    public AnalyzeBushCollectorAction()
    {
        setBaseDuration(1.5f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualBush", false);
        addEffect("bushFound", true);
    }

    public override void reset()
    {
        analyzed = false;
        targetBush = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return analyzed;
    }

    public override bool requiresInRange()
    {
        return true; 
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // Find bush
        float localRadius = (numTry/2) + radius;
        numTry++;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, localRadius);
        Collider2D closestCollider = null;
        float closestDist = 0;

        if (colliders == null)
        {
            return false;
        }
        foreach (Collider2D hit in colliders)
        {
            if (hit.tag != "Bush")
            {
                continue;
            }

            BushEntity bush = (BushEntity)hit.gameObject.GetComponent(typeof(BushEntity));
            if (bush.empty || bush.viewed)
            {
                continue;
            }
            if (closestCollider == null)
            {
                closestCollider = hit;
                closestDist = (closestCollider.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (hit.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closestCollider = hit;
                    closestDist = dist;
                }
            }
            Debug.DrawLine(closestCollider.gameObject.transform.position, agent.transform.position, Color.red, 3, false);
        }
        
        bool isClosest = closestCollider != null;
        if (isClosest)
        {
            targetBush = (BushEntity)closestCollider.gameObject.GetComponent(typeof(BushEntity));
            target = targetBush.gameObject;
            numTry = 1;
        } 
        // Bush too far
        if(numTry > 10)
        {
            // Evolution process
            Collector collector = (Collector)agent.GetComponent(typeof(Collector));
            if (collector.center.needCarriers())
            {
                collector.instanciateSuccessor("Carrier");
                return false;
            }

            if (collector.center.needHunters())
            {
                collector.instanciateSuccessor("Hunter");
                return false;
            }

            if (collector.center.needFishers())
            {
                collector.instanciateSuccessor("Fisher");
                return false;
            }

            collector.instanciateSuccessor("Farmer");
            return false;
        }
        return isClosest;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (targetBush.food <= 0)
        {
            disableBubbleIcon(agent);
            targetBush.turnEmptySprite();
            return false;
        }

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Collector collector = (Collector)agent.GetComponent(typeof(Collector));
            collector.energy -= energyCost;
            analyzed = true;
            if (targetBush.age < 3)
            {
                targetBush.viewed = true;
                return false;
            } else
            {
                collector.actualBush = targetBush;
            }
        }
        return true;
    }

}



