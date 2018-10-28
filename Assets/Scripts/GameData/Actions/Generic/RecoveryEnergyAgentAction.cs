using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RecoveryEnergyAgentAction : GoapAction
{
    private bool recovered = false;

    private float startTime = 0;
    public float recoveringDuration = 5; // seconds
    public int foodCost = 10;

    public RecoveryEnergyAgentAction()
    {
        addPrecondition("hasEnergy", false); // we need energy
        addEffect("hasEnergy", true);
    }


    public override void reset()
    {
        recovered = false;       
        startTime = 0;
    }

    public override bool isDone()
    {
        return recovered;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Agent abstractAgent = (Agent) agent.GetComponent(typeof(Agent));

        if(abstractAgent.house != null)
        {
            target = abstractAgent.house.gameObject;
        } else
        {
            HouseBuilding[] houses = (HouseBuilding[])FindObjectsOfType(typeof(HouseBuilding));
            foreach (HouseBuilding house in houses)
            {
                if (house.full || !house.blueprint.done)
                {
                    continue;
                }
                if (house.addAgent())
                {
                    abstractAgent.house = house;
                    break;
                }
            }
            if (abstractAgent.house == null)
            {
                Building building = new Building("Prefabs/Buildings/House", 100, 100, 3, 1);
                abstractAgent.center.addNewBuildingRequest(building);
                target = abstractAgent.center.gameObject;
            } else
            {
                target = abstractAgent.house.gameObject;
            }
        }

        // Debug line
        //Debug.DrawLine(target.transform.position, agent.transform.position, Color.blue, 3, false);
        return target != null;
    }

    public override bool perform(GameObject agent)
    {
        Agent abstractAgent = (Agent)agent.GetComponent(typeof(Agent));
        if (startTime == 0)
        {
            if(abstractAgent.warehouse.food < foodCost)
            {
                abstractAgent.waiting = true;
                return false;
            }
            abstractAgent.warehouse.food -= foodCost;
            abstractAgent.recovering = true;
            if(abstractAgent.house != null)
            {
                abstractAgent.house.enterAgentToRecover();
            } else
            {
                abstractAgent.center.enterAgentToRecover();
            }
            startTime = Time.time;
        }

        if (Time.time - startTime > recoveringDuration)
        {
            abstractAgent.energy = 100;
            abstractAgent.recovering = false;
            if (abstractAgent.house != null)
            {
                abstractAgent.house.exitAgentToRecover();
            }
            else
            {
                abstractAgent.center.exitAgentToRecover();
            }
            recovered = true;
        }
        return true;
    }

}


