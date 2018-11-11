using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class GoapAgent : MonoBehaviour
{
    private FSM stateMachine;

    private FSM.FSMState idleState; // Finds something to do
    private FSM.FSMState moveToState; // Moves to a target
    private FSM.FSMState performActionState; // Performs an action

    private HashSet<GoapAction> availableActions;
    private Queue<GoapAction> currentActions;

    private IGoap dataProvider; // Agent implement

    private GoapPlanner planner;

    public bool isSelected = false;

    void Start()
    {
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        findDataProvider();
        createIdleState();
        createMoveToState();
        createPerformActionState();
        stateMachine.pushState(idleState);
        loadActions();
    }


    void Update()
    {
        stateMachine.Update(gameObject);
    }

    public void addAction(GoapAction a)
    {
        availableActions.Add(a);
    }

    public GoapAction getAction(Type action)
    {
        foreach (GoapAction g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }

    public void removeAction(GoapAction action)
    {
        availableActions.Remove(action);
    }

    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void createIdleState()
    {
        idleState = (fsm, gameObj) => {
            // Get the world state and the goal 
            Dictionary<string, object> worldState = dataProvider.getWorldState();
            Dictionary<string, object> goal = dataProvider.createGoalState();

            // Generate the plan using GOAP Planner
            Queue<GoapAction> plan = planner.plan(gameObject, availableActions, worldState, goal);
            if (isSelected)
            {
                GameManager.instance.numActions = planner.numActions;
                GameManager.instance.numPaths = planner.numLeaves;
                GameManager.instance.numPossibilities = planner.numPossibilities;
                GameManager.instance.numRealIterations = planner.numRealIteration;
            }
            if (plan != null)
            {   
                // Plan found
                currentActions = plan;
                if (isSelected)
                {
                    generatePlanPanel();
                }
                dataProvider.planFound(goal, plan);
                fsm.popState(); // move to PerformAction state
                fsm.pushState(performActionState);

            }
            else
            {   
                // No plan
                Debug.Log("<color=orange>Failed Plan:</color>" + prettyPrint(goal));
                dataProvider.planFailed(goal);
                fsm.popState(); // move back to IdleAction state
                fsm.pushState(idleState);
            }

        };
    }

    private void createMoveToState()
    {
        moveToState = (fsm, gameObj) => {
            GoapAction action = currentActions.Peek();
            if (action.requiresInRange() && action.target == null && action.targetPosition.Equals(Vector3.zero))
            {                
                // No target or target position
                Debug.Log("<color=red>Fatal error:</color> Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                fsm.popState(); // move
                fsm.popState(); // perform
                fsm.pushState(idleState);
                return;
            }

            // Call agent move funtion
            if (dataProvider.moveAgent(action))
            {
                fsm.popState();
            }
        };
    }

    private void createPerformActionState()
    {
        performActionState = (fsm, gameObj) => {
            if (!hasActionPlan())
            {
                // No actions
                Debug.Log("<color=red>Done actions</color>");
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.isDone())
            {
                // Action is done
                currentActions.Dequeue();
                if (isSelected)
                {
                    generatePlanPanel();
                }
            }

            if (hasActionPlan())
            {
                // Perform the next action
                action = currentActions.Peek();
                bool inRange = action.requiresInRange() ? action.isInRange() : true;

                if (inRange)
                {
                    bool success = action.perform(gameObj);

                    if (!success)
                    {
                        // Action failed
                        fsm.popState();
                        fsm.pushState(idleState);
                        dataProvider.planAborted(action);
                    }
                }
                else
                {
                    // Move to target
                    fsm.pushState(moveToState);
                }

            }
            else
            {
                // No actions
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
            }

        };
    }

    private void findDataProvider()
    {
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }

    private void loadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach (GoapAction a in actions)
        {
            availableActions.Add(a);
        }
        Debug.Log("Found actions: " + prettyPrint(actions));
    }

    public static string prettyPrint(Dictionary<string, object> state)
    {
        string s = "";
        foreach (string key in state.Keys)
        {
            s += key + ":" + state[key].ToString();
            s += ", ";
        }
        return s;
    }

    public static string prettyPrint(Queue<GoapAction> actions)
    {
        string s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }

    public static string prettyPrint(GoapAction[] actions)
    {
        string s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }

    public static string prettyPrint(GoapAction action)
    {
        string s = "" + action.GetType().Name;
        return s;
    }

    public void generatePlanPanel()
    {
        if (currentActions != null)
        {
            GameManager.instance.addListToActionPlanPanel(this, currentActions);
            GameManager.instance.prepareMetricsPanel();
        }
    }

    // Follow camera
    void OnMouseDown()
    {
        MovementCamera movCam = Camera.main.gameObject.GetComponent<MovementCamera>();
        movCam.target = gameObject;
        
        GameManager.instance.numActions = planner.numActions;
        GameManager.instance.numPaths = planner.numLeaves;
        GameManager.instance.numPossibilities = planner.numPossibilities;
        GameManager.instance.numRealIterations = planner.numRealIteration;

        generatePlanPanel();
    }
}
