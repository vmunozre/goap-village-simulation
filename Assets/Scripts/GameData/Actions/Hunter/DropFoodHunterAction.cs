﻿using UnityEngine;

public class DropFoodHunterAction : GoapAction
{
    private bool droppedFood = false;

    private float startTime = 0;

    // Drop food
    public DropFoodHunterAction()
    {
        setActionName("Drop food");
        setBaseDuration(1.5f);
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

        target = hunter.warehouse.gameObject; 
        
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
            Hunter hunter = (Hunter)agent.GetComponent(typeof(Hunter));
            
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
                // Add request building hunting shed
                Building building = new Building("Prefabs/Buildings/huntingShed", 250, 150, 7, 2);
                hunter.center.addNewBuildingRequest(building);
                hunter.warehouse.food += hunter.food;
            } else
            {
                hunter.huntingShed.food += hunter.food;
            }
            
            hunter.food = 0;
            droppedFood = true;
        }
        return true;
    }
}

