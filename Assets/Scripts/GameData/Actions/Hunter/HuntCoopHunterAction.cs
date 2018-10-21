using UnityEngine;

public class HuntCoopHunterAction : GoapAction
{
    private bool hunt = false;
    private DeerEntity targetPrey = null;
    private int energyCost = 50;
    private float startTime = 0;
    public float huntDuration = 2.5f; // seconds

    public HuntCoopHunterAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasFood", false);
        addPrecondition("hasActualPrey", true);
        addPrecondition("hasDeadPrey", false);
        addPrecondition("hasCoopHunter", true);
        addPrecondition("isInPosition", true);
        addEffect("hasDeadPrey", true);
    }


    public override void reset()
    {
        hunt = false;
        targetPrey = null;
        startTime = 0;

    }

    public override bool isDone()
    {
        return hunt;
    }

    public override bool requiresInRange()
    {
        return true; // yes we need to be near a tree
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));

        targetPrey = hunter.actualPrey;
        if (targetPrey != null)
        {
            target = targetPrey.gameObject;
        }
        return targetPrey != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            Debug.Log("PREY DEAD!");
            if (!hunter.actualPrey.isDead)
            {
                hunter.actualPrey.killDeer();
            }
        }

        if (Time.time - startTime > huntDuration)
        {
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            hunter.energy -= energyCost;
            hunt = true;
        }

        return true;
    }

}


