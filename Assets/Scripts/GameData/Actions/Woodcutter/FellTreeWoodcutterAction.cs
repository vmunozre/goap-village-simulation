using UnityEngine;

public class FellTreeWoodcutterAction : GoapAction
{
    private bool chopped = false;
    private TreeEntity targetTree = null;
    // Timers
    public float choppedDuration = 5; // seconds
    private float startTime = 0;
    private int energyCost = 40;

    // Fell Tree
    public FellTreeWoodcutterAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("treeIsChopped", false);
        addPrecondition("hasActualTree", true);
        addPrecondition("hasWood", false);
        addEffect("treeIsChopped", true);
    }

    public override void reset()
    {
        chopped = false;
        targetTree = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return chopped;
    }

    public override bool requiresInRange()
    {
        return true;
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

        // Tree empty
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

            targetTree.chopped = true;
            targetTree.turnChoppedSprite();
            woodcutter.energy -= energyCost;
            chopped = true;
        }
        return true;
    }

}


