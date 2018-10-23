using UnityEngine;

public class FindPlaceBuilderAction : GoapAction
{
    private bool found = false;
    public Vector3 nextPosition;

    private float startTime = 0;
    private float searchDuration = 0.5f; // seconds
    private int energyCost = 2;

    // find settings
    private float radius = 2f;
    private float minMove = -1f;
    private float maxMove = 1f;
    public FindPlaceBuilderAction()
    {
        addPrecondition("hasEnergy", true);        
        addPrecondition("hasActualRequest", true);
        addPrecondition("hasActualBuilding", false);
        addEffect("hasActualBuilding", true);
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
        Builder builder = (Builder)agent.GetComponent(typeof(Builder));

        float posX = agent.transform.position.x + Random.Range(minMove, maxMove);
        float posY = agent.transform.position.y + Random.Range(minMove, maxMove);
        nextPosition = new Vector3(posX, posY, agent.transform.position.z);

        
        targetPosition = nextPosition;
        //Debug.DrawLine(targetPosition, agent.transform.position, Color.black, 3, false);

        return targetPosition != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > searchDuration)
        {
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            if (builder.actualBuilding == null)
            {
                builder.energy -= energyCost;
            }

            BoxCollider2D buildingCollider = builder.actualRequest.building.GetComponent<BoxCollider2D>();
            float offset = 0.1f;
            float divide = 2f;
            Vector2 pointA = new Vector2(agent.transform.position.x - ((buildingCollider.size.x / divide) + offset), agent.transform.position.y - ((buildingCollider.size.y / divide) + offset));
            Vector2 pointB = new Vector2(agent.transform.position.x + ((buildingCollider.size.x / divide) + offset), agent.transform.position.y + ((buildingCollider.size.y / divide) + offset));
            Vector2 pointC = new Vector2(agent.transform.position.x - ((buildingCollider.size.x / divide) + offset), agent.transform.position.y + ((buildingCollider.size.y / divide) + offset));
            Vector2 pointD = new Vector2(agent.transform.position.x + ((buildingCollider.size.x / divide) + offset), agent.transform.position.y - ((buildingCollider.size.y / divide) + offset));

            
            Debug.DrawLine(pointA, pointB, Color.black, 3, false);
            Debug.DrawLine(pointC, pointD, Color.black, 3, false);

            // Desactivación del collider del agente un momento
            Collider2D agentCollider = builder.GetComponent<BoxCollider2D>();
            agentCollider.enabled = false;
            Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);
            agentCollider.enabled = true;
            Collider2D closestCollider = null;
            float closestDist = 0;

            if (colliders == null)
            {
                return false;
            }
            if(colliders.Length <= 0)
            {
                GameObject build = Instantiate(builder.actualRequest.building, new Vector3(builder.transform.position.x, builder.transform.position.y, -0.1f), Quaternion.identity);
                SpriteRenderer sr = build.GetComponent<SpriteRenderer>();
                sr.sprite = build.GetComponent<HouseBuilding>().inConstructionSprite;
                build.GetComponent<HouseBuilding>().blueprint = builder.actualRequest;

                builder.actualBuilding = build;
                found = true;
            } else
            {
                return false;
            }

            //TODO si los colliders son árboles pedir que se talen y poner aqui la construcción
            /*
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
                found = true;

            }
            else
            {
                return false;
            }*/
        }
        return true;
    }


}




