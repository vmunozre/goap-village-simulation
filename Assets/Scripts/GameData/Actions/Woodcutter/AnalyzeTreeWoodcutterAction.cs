using UnityEngine;

public class AnalyzeTreeWoodcutterAction : GoapAction
{
    private bool analyzed = false;
    public TreeEntity targetTree;

    private float startTime = 0;
    public float analyzetDuration = 1.5f; // seconds
    private int energyCost = 5;


    // find settings
    private float radius = 1f;
    private int numTry = 1;

    public AnalyzeTreeWoodcutterAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasWood", false);
        addPrecondition("hasActualTree", false);
        addEffect("treeFound", true);
    }


    public override void reset()
    {
        analyzed = false;
        targetTree = null;
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
            if (hit.tag != "Tree")
            {
                continue;
            }

            TreeEntity tree = (TreeEntity)hit.gameObject.GetComponent(typeof(TreeEntity));
            if (tree.empty || tree.viewed || tree.chopped)
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
        if(isClosest)
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
            startTime = Time.time;
        }

        if (targetTree.wood <= 0)
        {
            targetTree.turnEmptySprite();
            return false;
        }

        if (Time.time - startTime > analyzetDuration)
        {
            Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));
            SawmillBuilding[] sawmills = (SawmillBuilding[])FindObjectsOfType(typeof(SawmillBuilding));
            if (sawmills != null && sawmills.Length <= 0)
            {
                CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
                if (centers != null && centers.Length > 0)
                {
                    Building building = new Building("Prefabs/Buildings/Sawmill", 200, 150, 25, 2);

                    centers[0].addNewBuildingRequest(building);

                }
            }

            woodcutter.energy -= energyCost;
            analyzed = true;
            if (targetTree.age < 3)
            {
                targetTree.viewed = true;
                return false;
            }
            else
            {
                woodcutter.actualTree = targetTree;
                
            }
        }
        return true;
    }

}



