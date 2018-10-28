using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilding : BaseBuilding
{
    public int capacity = 4;
    public int agentCount = 0;
    public bool full;
    public WarehouseEntity warehouse;

	// Use this for initialization
	void Start () {
        WarehouseEntity[] warehouses = (WarehouseEntity[])FindObjectsOfType(typeof(WarehouseEntity));
        if (warehouses.Length > 0)
        {
            warehouse = warehouses[0];
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

    }

    public void exitAgentToRecover()
    {

    }
}
