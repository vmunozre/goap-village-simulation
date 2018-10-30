using UnityEngine;

public class DropResourcesBuilderAction : GoapAction
{
    private bool droppedResources = false;
    private GameObject targetBuilding;

    private float startTime = 0;
    public float dropDuration = 1.5f; // seconds

    private int energyCost = 10;
    public DropResourcesBuilderAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualRequest", true);
        addPrecondition("hasActualBuilding", true);
        addPrecondition("hasResources", true);
        addPrecondition("buildingSupply", false);
        addEffect("hasResources", false);
        addEffect("buildingSupply", true);
    }

    public override void reset()
    {
        droppedResources = false;
        targetBuilding = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedResources;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Builder builder = (Builder)agent.GetComponent(typeof(Builder));

        bool checkActualBuilding = builder.actualBuilding != null;
        if (checkActualBuilding)
        {
            targetBuilding = builder.actualBuilding;
            target = targetBuilding;
        }
        // Debug line
        //Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return checkActualBuilding;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if (Time.time - startTime > dropDuration)
        {
            disableBubbleIcon(agent);
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();

            building.blueprint.actualWood += builder.wood;
            building.blueprint.actualStone += builder.stone;
            builder.wood = 0;
            builder.stone = 0;

            builder.energy -= energyCost;

            if(building.blueprint.hasAllResources())
            {
                droppedResources = true;
                return true;
            } else
            {
                return false;
            }
        }
        return true;
    }
}

