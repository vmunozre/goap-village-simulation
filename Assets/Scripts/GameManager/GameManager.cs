using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float actualMuti = 1;

    private CenterEntity center = null;
    private WarehouseEntity warehouse = null;

    // metrics
    public int numActions = 0;
    public int numPossibilities = 0;
    public int numRealIterations = 0;
    public int numPaths = 0;

    public GameObject panelActionPlan;
    public GameObject panelMetrics;
    public GameObject panelDeselectAgent;
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

        // Unselect target
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            deselectAgent();
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

    public void deselectAgent()
    {
        MovementCamera movCam = Camera.main.gameObject.GetComponent<MovementCamera>();
        movCam.target = null;
        agentSelected.isSelected = false;
        agentSelected = null;
        toggleVisiblePanels(false);
    }

    public void addListToActionPlanPanel(GoapAgent agent, Queue<GoapAction> queue)
    {
        if (!isVisibleActionPlanPanel())
        {
            toggleVisiblePanels(true);            
        }
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

    public void prepareMetricsPanel()
    {
        foreach (Transform child in panelMetrics.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject metric1 = (GameObject)Instantiate(Resources.Load("Prefabs/UI/metric"));
       
        metric1.GetComponent<Text>().text = " Nº Actions: " + numActions;
        metric1.transform.SetParent(panelMetrics.transform);
        metric1.transform.localScale = new Vector3(1, 1, 1);

        GameObject metric2 = (GameObject)Instantiate(Resources.Load("Prefabs/UI/metric"));
       
        metric2.GetComponent<Text>().text = " Nº paths: " + numPaths;
        metric2.transform.SetParent(panelMetrics.transform);
        metric2.transform.localScale = new Vector3(1, 1, 1);

        GameObject metric3 = (GameObject)Instantiate(Resources.Load("Prefabs/UI/metric"));
        
        metric3.GetComponent<Text>().text = " Nº Possibilities: " + numPossibilities;
        metric3.transform.SetParent(panelMetrics.transform);
        metric3.transform.localScale = new Vector3(1, 1, 1);

        GameObject metric4 = (GameObject)Instantiate(Resources.Load("Prefabs/UI/metric"));
   
        metric4.GetComponent<Text>().text = " Nº real iter: " + numRealIterations;
        metric4.transform.SetParent(panelMetrics.transform);
        metric4.transform.localScale = new Vector3(1, 1, 1);
    }

    private void toggleVisiblePanels(bool _active)
    {
        panelActionPlan.transform.parent.parent.parent.gameObject.SetActive(_active);
        panelMetrics.transform.parent.gameObject.SetActive(_active);
        panelDeselectAgent.SetActive(_active);
    }
    private bool isVisibleActionPlanPanel()
    {
        return panelActionPlan.transform.parent.parent.parent.gameObject.activeSelf;
    }
}
