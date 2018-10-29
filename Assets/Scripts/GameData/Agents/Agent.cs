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

    private Animator animator;
    // Places
    public CenterEntity center = null;
    public WarehouseEntity warehouse = null;
    
    public HouseBuilding house = null;
    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!isAdult)
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            moveSpeed = 0;
            bornDuration = 6f;
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
        if(house == null)
        {
            Building building = new Building("Prefabs/Buildings/House", 100, 100, 20, 1);
            center.addNewBuildingRequest(building);
        }
    }


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
    /**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
    public abstract HashSet<KeyValuePair<string, object>> getWorldState();

    /**
	 * Implement in subclasses
	 */
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();


    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.
    }

    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
    }

    public void actionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void planAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
    }

    public bool moveAgent(GoapAction nextAction)
    {
        //animator.SetBool("isMoving", true);
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
        // move towards the NextAction's target
        float step = moveSpeed * Time.deltaTime;
        Vector3 actualTarget = new Vector3(position.x, position.y, transform.position.z);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, actualTarget, step);
        Vector3 toCompare = new Vector3(position.x, position.y, transform.position.z);
        if (gameObject.transform.position.Equals(toCompare))
        {
            // we are at the target location, we are done
           // animator.SetBool("isMoving", false);
            nextAction.setInRange(true);
            return true;
        }
        else
            return false;
    }
}

