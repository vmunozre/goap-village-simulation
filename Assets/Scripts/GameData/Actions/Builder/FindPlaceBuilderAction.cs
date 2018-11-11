using UnityEngine;

public class FindPlaceBuilderAction : GoapAction
{
    private bool found = false;
    public Vector3 nextPosition;

    private float startTime = 0;
    private int energyCost = 2;

    // find settings
    private float minMove = -1f;
    private float maxMove = 1f;
    public FindPlaceBuilderAction()
    {
        setActionName("Find building place");
        setBaseDuration(0.5f);
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
        // TODO Find according places (sawmill == forest)
        // Move to random position
        float posX = agent.transform.position.x + Random.Range(minMove, maxMove);
        float posY = agent.transform.position.y + Random.Range(minMove, maxMove);
        nextPosition = new Vector3(posX, posY, agent.transform.position.z);

        targetPosition = nextPosition;
        //Debug.DrawLine(targetPosition, agent.transform.position, Color.black, 3, false);

        return targetPosition != Vector3.zero;
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
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            if (builder.actualBuilding == null)
            {
                builder.energy -= energyCost;
            }
            // Check collider
            BoxCollider2D buildingCollider = builder.actualRequest.building.GetComponent<BoxCollider2D>();
            float offset = 0.1f;
            float divide = 2f;
            Vector2 pointA = new Vector2(agent.transform.position.x - ((buildingCollider.size.x / divide) + offset), agent.transform.position.y - ((buildingCollider.size.y / divide) + offset));
            Vector2 pointB = new Vector2(agent.transform.position.x + ((buildingCollider.size.x / divide) + offset), agent.transform.position.y + ((buildingCollider.size.y / divide) + offset));
            Vector2 pointC = new Vector2(agent.transform.position.x - ((buildingCollider.size.x / divide) + offset), agent.transform.position.y + ((buildingCollider.size.y / divide) + offset));
            Vector2 pointD = new Vector2(agent.transform.position.x + ((buildingCollider.size.x / divide) + offset), agent.transform.position.y - ((buildingCollider.size.y / divide) + offset));

            
            Debug.DrawLine(pointA, pointB, Color.black, 3, false);
            Debug.DrawLine(pointC, pointD, Color.black, 3, false);
            
            Collider2D agentCollider = builder.GetComponent<BoxCollider2D>();
            agentCollider.enabled = false;
            Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, pointB);
            agentCollider.enabled = true;

            if (colliders == null)
            {
                return false;
            }
            if(colliders.Length <= 0)
            {
                GameObject build = Instantiate(builder.actualRequest.building, new Vector3(builder.transform.position.x, builder.transform.position.y, -0.1f), Quaternion.identity);
                SpriteRenderer sr = build.GetComponent<SpriteRenderer>();
                sr.sprite = build.GetComponent<BaseBuilding>().inConstructionSprite;
                build.GetComponent<BaseBuilding>().blueprint = builder.actualRequest;

                builder.actualBuilding = build;
                found = true;
            } else
            {
                return false;
            }

            //TODO Request fell tree
           
        }
        return true;
    }


}




