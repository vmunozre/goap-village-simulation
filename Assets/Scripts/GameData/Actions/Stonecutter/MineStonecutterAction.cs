using UnityEngine;

public class MineStonecutterAction : GoapAction
{
    private bool mined = false;
    private QuarryEntity targetQuarry = null;

    private float startTime = 0;
    private int energyCost = 30;

    // find settings
    private float radius = 5f;
    private int numTry = 1;

    // Mine stone
    public MineStonecutterAction()
    {
        setBaseDuration(10f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasStone", false);
        addEffect("hasStone", true);
    }

    public override void reset()
    {
        mined = false;
        targetQuarry = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return mined;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    // Find quarry
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Stonecutter stonecutter = (Stonecutter)agent.GetComponent(typeof(Stonecutter));
        if(stonecutter.actualQuarry != null)
        {
            targetQuarry = stonecutter.actualQuarry;
            if (targetQuarry != null)
            {
                target = targetQuarry.gameObject;
            }
        } else
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
                if (hit.tag != "Quarry")
                {
                    continue;
                }

                QuarryEntity quarry = (QuarryEntity)hit.gameObject.GetComponent(typeof(QuarryEntity));
                if (quarry.full)
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
                Debug.DrawLine(closestCollider.gameObject.transform.position, agent.transform.position, Color.gray, 3, false);
            }

            bool isClosest = closestCollider != null;
            if (isClosest)
            {
                targetQuarry = (QuarryEntity)closestCollider.gameObject.GetComponent(typeof(QuarryEntity));
                target = targetQuarry.gameObject;
                numTry = 1;
            }
        }
        
        return targetQuarry != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            Stonecutter stonecutter = (Stonecutter)agent.GetComponent(typeof(Stonecutter));
            startTime = Time.time;
            if (stonecutter.actualQuarry == null)
            {
                bool checkQuarry = targetQuarry.addStoneCutters();
                if (checkQuarry)
                {
                    stonecutter.actualQuarry = targetQuarry;
                } else
                {
                    return false;
                }
            }
        }

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Stonecutter stonecutter = (Stonecutter)agent.GetComponent(typeof(Stonecutter));

            stonecutter.stone += Random.Range(10, 50);
            stonecutter.energy -= energyCost;
            mined = true;
        }
        return true;
    }

}


