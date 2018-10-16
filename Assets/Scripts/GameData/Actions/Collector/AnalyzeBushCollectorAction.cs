using UnityEngine;

public class AnalyzeBushCollectorAction : GoapAction
{
    private bool analyzed = false;
    public BushEntity targetBush;

    private float startTime = 0;
    public float analyzetDuration = 1.5f; // seconds
    private int energyCost = 5;


    // find settings
    private float radius = 5f;
    private int numTry = 1;

    public AnalyzeBushCollectorAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualBush", false);
        addEffect("bushFound", true);
    }


    public override void reset()
    {
        analyzed = false;
        targetBush = null;
        numTry = 1;
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
        float localRadius = numTry * radius;
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
        targetBush = (BushEntity)closestCollider.gameObject.GetComponent(typeof(BushEntity));
        target = targetBush.gameObject;
        bool isClosest = closestCollider != null;
        return isClosest;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (targetBush.food <= 0)
        {           
            targetBush.turnEmptySprite();
            return false;
        }

        if (Time.time - startTime > analyzetDuration)
        {
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



