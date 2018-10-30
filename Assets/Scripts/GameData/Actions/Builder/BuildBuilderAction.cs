using UnityEngine;

public class BuildBuilderAction : GoapAction
{
    private bool built = false;
    private GameObject targetBuilding;

    private float startTime = 0;
    private float timeEnergyLoss = 5;
    private int energyCost = 5;

    public BuildBuilderAction()
    {
        addPrecondition("hasEnergy", true);
        addPrecondition("hasActualRequest", true);
        addPrecondition("hasActualBuilding", true);        
        addPrecondition("buildingSupply", true);
        addPrecondition("hasResources", false);
        addEffect("buildComplete", true);        
    }

    public override void reset()
    {
        built = false;
        targetBuilding = null;
        startTime = 0;
    }

    public override bool isDone()
    {
        return built;
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
//            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();            
            targetBuilding = builder.actualBuilding;
            target = targetBuilding;
        }
        // Debug line
        //Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return checkActualBuilding;
    }

    public override bool perform(GameObject agent)
    {
        Builder builder = (Builder)agent.GetComponent(typeof(Builder));
        BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();
        if (startTime == 0)
        {
            enableBubbleIcon(agent);
            startTime = Time.time;
        }

        if((Time.time - startTime) > timeEnergyLoss)
        {
            building.blueprint.progress += 1;
            builder.energy -= energyCost;
            if(builder.energy <= 0)
            {
                disableBubbleIcon(agent);
                return false;
            }
            startTime = Time.time;
        }

        if (building.blueprint.progress >= building.blueprint.buildEffort)
        {
            disableBubbleIcon(agent);
            SpriteRenderer sr = builder.actualBuilding.GetComponent<SpriteRenderer>();
            sr.sprite = building.normalSprite;
            building.blueprint.done = true;
            built = true;
        }
        return true;
    }
}

