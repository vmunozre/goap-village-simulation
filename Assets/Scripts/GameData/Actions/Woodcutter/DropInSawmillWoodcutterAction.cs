using UnityEngine;

public class DropInSawmillWoodcutterAction : GoapAction
{
    private bool droppedWood = false;
    private float startTime = 0;

    // Drop wood
    public DropInSawmillWoodcutterAction()
    {
        setActionName("Drop in sawmill");
        setBaseDuration(1.5f);
        changeDefaultCost(0.5f);
        addPrecondition("hasWood", true);
        addEffect("hasWood", false);
        addEffect("collectWood", true);
    }

    public override void reset()
    {
        droppedWood = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedWood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Woodcutter woodcutter = (Woodcutter)agent.GetComponent(typeof(Woodcutter));

        if (woodcutter.sawmill != null)
        {
            target = woodcutter.sawmill.gameObject;
        }
        // Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return target != null;
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
            if (woodcutter.sawmill != null)
            {
                // Drop in sawmill
                woodcutter.sawmill.wood += woodcutter.wood;
            }
            
            woodcutter.wood = 0;
            droppedWood = true;
        }
        return true;
    }
}

