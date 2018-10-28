﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilding : BaseBuilding
{
    public int capacity = 4;
    public int agentCount = 0;
    public int actualAgents = 0;
    public int bornCost = 45;

    public bool full;
    public WarehouseEntity warehouse;
    public CenterEntity center;
	// Use this for initialization
	void Start () {
        WarehouseEntity[] warehouses = (WarehouseEntity[])FindObjectsOfType(typeof(WarehouseEntity));
        if (warehouses.Length > 0)
        {
            warehouse = warehouses[0];
        }
        CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
        if (centers.Length > 0)
        {
            center = centers[0];
        }
    }
	
    public bool addAgent()
    {
        bool result = false;
        if (!full)
        {
            agentCount++;
            full = (agentCount >= capacity);
            result = true;
        }
        return result;
    }

    public void enterAgentToRecover()
    {
        actualAgents++;
        procreate();
    }

    public void exitAgentToRecover()
    {
        actualAgents--;
    }

    private void procreate()
    {
        if (actualAgents >= 2 && (actualAgents % 2 == 0) && warehouse.food >= bornCost)
        {
            int rate = Random.Range(1, 100);

            if (rate <= 35 && center.needStoneCutters())
            {
                rate = Random.Range(1, 100);
                if (rate < 15 && center.needBuilders())
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Builder"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
                else
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Stonecutter"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
            }
            else
            {
                rate = Random.Range(1, 100);

                if (rate < 40 && center.needWoodcutters())
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Woodcutter"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
                else
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Collector"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
            }
            warehouse.food -= bornCost;
        }
    }
}
