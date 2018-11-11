using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float actualMuti = 1;

    private CenterEntity center = null;
    private WarehouseEntity warehouse = null;

    public GameObject panelActionPlan;
    public GoapAgent agentSelected;

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
        if (centers.Length > 0)
        {
            center = centers[0];
        }

        WarehouseEntity[] warehouses = (WarehouseEntity[])FindObjectsOfType(typeof(WarehouseEntity));
        if (warehouses.Length > 0)
        {
            warehouse = warehouses[0];
        }

    }
	
	void Update () {
        // Speed controllers
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            actualMuti = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            actualMuti = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            actualMuti = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            actualMuti = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            actualMuti = 10;
        }

        // Force procreate
        if (Input.GetKeyDown(KeyCode.P))
        {
            center.procreateRule();
        }

        // Add resources x100
        if (Input.GetKeyDown(KeyCode.U))
        {
            warehouse.food += 100;
            warehouse.wood += 100;
            warehouse.stone += 100;
        }
    }

    public void addListToActionPlanPanel(GoapAgent agent, Queue<GoapAction> queue)
    {
        if(agentSelected != null)
        {
            agentSelected.isSelected = false;
        }
        agentSelected = agent;
        agentSelected.isSelected = true;
        foreach (Transform child in panelActionPlan.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (GoapAction action in queue)
        {
            GameObject inst = (GameObject)Instantiate(Resources.Load("Prefabs/UI/action"));
          
            inst.GetComponent<ActionPlanUI>().setContent(action.actionName, action.uiImage);
            inst.transform.SetParent(panelActionPlan.transform);
            inst.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
