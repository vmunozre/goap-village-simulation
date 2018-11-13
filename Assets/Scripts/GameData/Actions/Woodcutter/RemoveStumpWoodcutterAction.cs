using UnityEngine;

public class RemoveStumpWoodcutterAction : GoapAction
{
    private bool removed = false;
    private TreeEntity targetTree = null;

    private float startTime = 0;
    private int energyCost = 30;

    // find settings
    private float radius = 5f;
    private int numTry = 1;

    // Remove stump tree
    public RemoveStumpWoodcutterAction()
    {
        setActionName("Remove Stump");
        changeDefaultCost(0.5f);
        setBaseDuration(5f);
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualTree", false);        
        addPrecondition("hasWood", false);        
        addEffect("hasWood", true);
        addEffect("treeFound", true);
    }


    public override void reset()
    {
        removed = false;
        targetTree = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return removed;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        // Find bushes in radius
        float localRadius = numTry + radius;
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
            if (hit.tag != "Tree")
            {
                continue;
            }

            TreeEntity tree = (TreeEntity)hit.gameObject.GetComponent(typeof(TreeEntity));
            if (tree == null || !tree.empty)
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
            Debug.DrawLine(closestCollider.gameObject.transform.position, agent.transform.position, Color.green, 3, false);
        }
        bool isClosest = closestCollider != null;
        if (isClosest)
        {
            targetTree = (TreeEntity)closestCollider.gameObject.GetComponent(typeof(TreeEntity));
            target = targetTree.gameObject;
            numTry = 1;
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

        if (Time.time - startTime > duration)
        {
            disableBubbleIcon(agent);
            Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));
            if(targetTree != null && targetTree.gameObject != null)
            {
                Destroy(targetTree.gameObject);
            }
            woodcutter.wood += 20;
            woodcutter.energy -= energyCost;
            removed = true;
        }
        return true;
    }

}


