using UnityEngine;

public class BuildBuilderAction : GoapAction
{
    private bool built = false;
    private GameObject targetBuilding;

    private float startTime = 0;
    public float buildDuration = 10f; // seconds
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
            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();
            buildDuration = Mathf.Max(1, building.blueprint.timeCost - building.blueprint.progress);
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
            startTime = Time.time;
        }

        if((Time.time - startTime) % timeEnergyLoss == 0)
        {
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();
            building.blueprint.progress += timeEnergyLoss;
            builder.energy -= energyCost;
            if(builder.energy <= 0)
            {
                return false;
            }
        }

        if (Time.time - startTime > buildDuration)
        {
            Builder builder = (Builder)agent.GetComponent(typeof(Builder));
            BaseBuilding building = builder.actualBuilding.GetComponent<BaseBuilding>();

            if(building.blueprint.progress >= building.blueprint.timeCost)
            {
                SpriteRenderer sr = builder.actualBuilding.GetComponent<SpriteRenderer>();
                sr.sprite = building.normalSprite;

                builder.actualRequest = null;
                builder.actualBuilding = null;

                built = true;
                return true;
            } else
            {
                return false;
            }

        }
        return true;
    }
}

