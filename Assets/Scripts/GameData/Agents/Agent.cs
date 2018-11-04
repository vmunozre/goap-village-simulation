using UnityEngine;
using System.Collections.Generic;

public abstract class Agent : MonoBehaviour, IGoap
{
    // Basic data
    public float moveSpeed = 1;
    public int energy = 100;
   
    public bool recovering = false;
    public bool isMoving = false;
    public bool waiting = false;

    public bool isAdult = false;
    private float startTimerBorn = 0f;
    private float bornDuration = 0f;

    // Places
    public CenterEntity center = null;
    public WarehouseEntity warehouse = null;
    
    public HouseBuilding house = null;

    void Awake()
    {        
        if (!isAdult)
        {
            // Turn child
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            moveSpeed = 0;
            bornDuration = 6f;
        }
        // Find center and warehouse
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

        HouseBuilding[] houses = (HouseBuilding[])FindObjectsOfType(typeof(HouseBuilding));
        foreach(HouseBuilding hos in houses)
        {
            if (hos.full || !hos.blueprint.done)
            {
                continue;
            }
            if (hos.addAgent())
            {
                house = hos;
                break;
            }
        }

        // No house, add house request
        if(house == null)
        {
            Building building = new Building("Prefabs/Buildings/House", 100, 100, 3, 1);
            center.addNewBuildingRequest(building);
        }
    }

    // Transform in adult agent
    public void checkIsAdult()
    {
        if (!isAdult)
        {
            if (startTimerBorn == 0)
            {
                startTimerBorn = Time.time;
            }

            if (Time.time - startTimerBorn > bornDuration)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                moveSpeed = 1;
                isAdult = true;
            }
            else
            {
                float scale = Mathf.Min(1f, transform.localScale.x + 0.01f);
                transform.localScale = new Vector3(scale, scale, 1f);
            }
        } else
        {
            moveSpeed = 1;
        }
    }

    public abstract Dictionary<string, object> getWorldState();

    public abstract Dictionary<string, object> createGoalState();

    public void planFailed(Dictionary<string, object> failedGoal)
    {

    }

    public void planFound(Dictionary<string, object> goal, Queue<GoapAction> actions)
    {
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
    }

    public void actionsFinished()
    {
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void planAborted(GoapAction aborter)
    {
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
    }

    public bool moveAgent(GoapAction nextAction)
    {
        Vector3 position = Vector3.zero;
        if(nextAction.target != null)
        {
            position = nextAction.target.transform.position;
        }
        if(nextAction.targetPosition != Vector3.zero)
        {            
            position = nextAction.targetPosition;
            
        }
        if (position.Equals(Vector3.zero))
        {
            
            return false;
        }
        
        float step = moveSpeed * Time.deltaTime;
        Vector3 actualTarget = new Vector3(position.x, position.y, transform.position.z);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, actualTarget, step);
        Vector3 toCompare = new Vector3(position.x, position.y, transform.position.z);
        if (gameObject.transform.position.Equals(toCompare))
        {
            nextAction.setInRange(true);
            return true;
        }
        else
            return false;
    }
}

