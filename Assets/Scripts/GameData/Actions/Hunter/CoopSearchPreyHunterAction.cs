using UnityEngine;

public class CoopSearchPreyHunterAction : GoapAction
{
    private bool found = false;
    public Vector3 nextPosition;


    private float startTime = 0;
    private float searchDuration = 1.5f; // seconds
    private int energyCost = 2;
    private HerdEntity trendHerd = null;
    // find settings
    private float radius = 2f;
    private float minMove = -0.4f;
    private float maxMove = 0.8f;
    public CoopSearchPreyHunterAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualPrey", false);
        addPrecondition("hasDeadPrey", false);
        addPrecondition("hasCoopHunter", true);
        addEffect("hasActualPrey", true);
        addEffect("preyFound", true);
    }


    public override void reset()
    {
        found = false;
        nextPosition = Vector3.zero;
        startTime = 0;
    }

    public override bool isDone()
    {
        return found;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {

        if (trendHerd == null)
        {
            trendHerd = getTrendHerd();
        }

        if (trendHerd != null)
        {
            float posX = agent.transform.position.x - Random.Range(minMove, maxMove);
            if (trendHerd.transform.position.x > agent.transform.position.x)
            {
                posX = agent.transform.position.x + Random.Range(minMove, maxMove);
            }

            float posY = agent.transform.position.y - Random.Range(minMove, maxMove);
            if (trendHerd.transform.position.y > agent.transform.position.y)
            {
                posY = agent.transform.position.y + Random.Range(minMove, maxMove);
            }

            nextPosition = new Vector3(posX, posY, agent.transform.position.z);
            targetPosition = nextPosition;
            Debug.DrawLine(targetPosition, agent.transform.position, Color.black, 3, false);
        }

        return trendHerd != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > searchDuration)
        {
            disableBubbleIcon(agent);
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            if (hunter.actualPrey == null)
            {
                hunter.energy -= energyCost;
            } else
            {
                found = true;
                return true;
            }
            Collider2D[] colliders = Physics2D.OverlapCircleAll(agent.transform.position, radius);
            Collider2D closestCollider = null;
            float closestDist = 0;

            if (colliders == null)
            {
                return false;
            }
            foreach (Collider2D hit in colliders)
            {
                if (hit.tag != "Deer")
                {
                    continue;
                }

                DeerEntity deer = (DeerEntity)hit.gameObject.GetComponent(typeof(DeerEntity));
                if (!deer.isAdult || deer.isDead)
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
            }

            bool isClosest = closestCollider != null;
            if (isClosest)
            {
                hunter.actualPrey = (DeerEntity)closestCollider.gameObject.GetComponent(typeof(DeerEntity));
                hunter.coopHunter.actualPrey = hunter.actualPrey;
                found = true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private HerdEntity getTrendHerd()
    {
        HerdEntity[] herds = (HerdEntity[])FindObjectsOfType(typeof(HerdEntity));
        if (herds == null)
        {
            return null;
        }
        int index = Random.Range(0, Mathf.Max(0, herds.Length - 1));

        if (herds.Length > 0)
        {
            Debug.Log("HERD FOUND");
            return herds[index];
        }
        return null;

    }

}



