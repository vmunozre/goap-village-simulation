using UnityEngine;

public class CollectFoodCollectorAction : GoapAction
{
    private bool collected = false;
    private BushEntity targetBush;

    private float startTime = 0;
    public float collectDuration = 3; // seconds
    private int energyCost = 30;


    // find settings
    private float radius = 100f;
    private int numTry = 1;

    public CollectFoodCollectorAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false); 
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
        return true; // yes we need to be near a tree
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        float localRadius = numTry * radius;
        numTry++;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, localRadius);
        Collider2D closestCollider = null;
        float closestDist = 0;
        
        if(colliders == null)
        {
            return false;
        }
        foreach(Collider2D hit in colliders)
        {
            if (hit.tag != "Bush")
            {
                continue;
            }

            BushEntity bush = (BushEntity)hit.gameObject.GetComponent(typeof(BushEntity));
            if(bush.empty)
            {
                continue;
            }
            if (closestCollider == null)
            {
                closestCollider = hit;
                closestDist = (closestCollider.gameObject.transform.position - agent.transform.position).magnitude;                
            } else
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
        targetBush = (BushEntity) closestCollider.gameObject.GetComponent(typeof(BushEntity));
        target = targetBush.gameObject;
        bool isClosest = closestCollider != null;
        if (isClosest)
        {
            numTry = 1;
        }
        return closestCollider != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }
        
        if(targetBush.food <= 0)
        {
            targetBush.empty = true;
            targetBush.turnEmptySprite();
            return false;
        }

        if (Time.time - startTime > collectDuration)
        {
            // finished cutting
            Collector collector = (Collector)agent.GetComponent(typeof(Collector));

            int food = 101;
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


