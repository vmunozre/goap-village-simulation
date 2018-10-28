using UnityEngine;

public class DropFoodHunterAction : GoapAction
{
    private bool droppedFood = false;

    private float startTime = 0;
    public float dropDuration = 1.5f; // seconds

    public DropFoodHunterAction()
    {
        addPrecondition("hasFood", true);
        addEffect("hasFood", false);
        addEffect("collectFood", true);
    }


    public override void reset()
    {
        droppedFood = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return droppedFood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));

        if(hunter.huntingShed != null)
        {
            target = hunter.huntingShed.gameObject;
        } else
        {
            target = hunter.warehouse.gameObject;
        }
        
        // Debug line
        // Debug.DrawLine(target.transform.position, agent.transform.position, Color.yellow, 3, false);
        return target != null;
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > dropDuration)
        {
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            if (hunter.huntingShed != null)
            {
                hunter.huntingShed.food += hunter.food;
            }
            else
            {
                HuntingShedBuilding[] huntingSheds = (HuntingShedBuilding[])FindObjectsOfType(typeof(HuntingShedBuilding));
                foreach (HuntingShedBuilding shed in huntingSheds)
                {
                    if (!shed.blueprint.done)
                    {
                        continue;
                    }
                    hunter.huntingShed = shed;
                    hunter.huntingShed.hunters++;
                    break;
                }
                if (hunter.huntingShed == null)
                {
                    Building building = new Building("Prefabs/Buildings/huntingShed", 250, 150, 30, 2);
                    hunter.center.addNewBuildingRequest(building);
                    hunter.warehouse.food += hunter.food;
                } else
                {
                    hunter.huntingShed.food += hunter.food;
                }
            }
            hunter.food = 0;
            droppedFood = true;
        }
        return true;
    }
}

