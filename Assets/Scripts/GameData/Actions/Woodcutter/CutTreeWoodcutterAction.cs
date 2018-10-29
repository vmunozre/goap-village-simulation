using UnityEngine;

public class CutTreeWoodcutterAction : GoapAction
{
    private bool clipped = false;
    private TreeEntity targetTree = null;

    private float startTime = 0;
    public float choppedDuration = 5; // seconds
    private int energyCost = 20;

    public CutTreeWoodcutterAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualTree", true);
        addPrecondition("treeIsChopped", true);
        addPrecondition("hasWood", false);
        addEffect("hasWood", true);
    }


    public override void reset()
    {
        clipped = false;
        targetTree = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return clipped;
    }

    public override bool requiresInRange()
    {
        return true; // yes we need to be near a tree
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));

        targetTree = woodcutter.actualTree;
        if (targetTree != null)
        {
            target = targetTree.gameObject;
        }
        return targetTree != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (targetTree.wood <= 0)
        {
            disableBubbleIcon(agent);
            Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));
            targetTree.turnEmptySprite();
            woodcutter.actualTree = null;
            return false;
        }

        if (Time.time - startTime > choppedDuration)
        {
            disableBubbleIcon(agent);
            Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));
            // finished cutting
            int wood = 30;
            if ((targetTree.wood - wood) >= 0)
            {
                woodcutter.wood += wood;
                targetTree.wood -= wood;
            }
            else
            {
                woodcutter.wood += targetTree.wood;
                targetTree.wood = 0;
            }
            woodcutter.energy -= energyCost;
            clipped = true;
        }
        return true;
    }

}


