using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoapPlanner
{

    // Plan what sequence of actions can fulfill the goal.
    public Queue<GoapAction> plan(GameObject agent,
                                  HashSet<GoapAction> availableActions,
                                  Dictionary<string, object> worldState,
                                  Dictionary<string, object> goal)
    {
        // Reset the actions 
        foreach (GoapAction a in availableActions)
        {
            a.doReset();
        }

        // Check what actions can run using their checkProceduralPrecondition
        HashSet<GoapAction> usableActions = new HashSet<GoapAction>();
        foreach (GoapAction a in availableActions)
        {
            if (a.checkProceduralPrecondition(agent))
                usableActions.Add(a);
        }

        // Node leaves
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, worldState, null);
        // Build graph
        bool success = optBuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            // No plan
            Debug.Log("NO PLAN");
            return null;
        }

        // Get a list of actions
        List<GoapAction> result = new List<GoapAction>();
        Node n = leaves[leaves.Count -1];
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action); // insert the action in the front
            }
            n = n.parent;
        }
        
        // Add actions to the queue
        Queue<GoapAction> queue = new Queue<GoapAction>();
        foreach (GoapAction a in result)
        {
            queue.Enqueue(a);
        }

        return queue;
    }

    // Found plan
    private bool optBuildGraph(Node parent, List<Node> leaves, HashSet<GoapAction> usableActions, Dictionary<string, object> goal)
    {
        bool found = false;
        // Sort the actions
        List<GoapAction> actions = usableActions.ToList<GoapAction>();
        actions.Sort((x, y) => x.checkCoincidencies(parent.state).CompareTo(y.checkCoincidencies(parent.state)));

        foreach (GoapAction action in actions)
        {
            // If the parent state has the conditions for this action's preconditions, we can use it here
            if (inState(action.Preconditions, parent.state))
            {
                // Apply the action's effects to the parent state
                Dictionary<string, object> currentState = new Dictionary<string, object>(populateState(parent.state, action.Effects));
                Node node = new Node(parent, parent.runningCost + action.cost, currentState, action);
                // If the we have plan and the cost is higher, stop this way
                if (leaves.Count > 0 && node.runningCost >= leaves[leaves.Count - 1].runningCost)
                {
                    continue;
                }
                if (inState(goal, currentState))
                {
                    // Found solution
                    leaves.Add(node);
                    found = true;
                }
                else
                {
                    // Test other branches
                    HashSet<GoapAction> subset = actionSubset(usableActions, action);
                    bool founded = optBuildGraph(node, leaves, subset, goal);
                    if (founded)
                        found = true;
                }
            }
        }
        return found;
    }

    // return subset without removeMe item
    private HashSet<GoapAction> actionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
    {
        HashSet<GoapAction> subset = new HashSet<GoapAction>();
        foreach (GoapAction a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

    // Check if all items in 'test' are in 'state'
    private bool inState(Dictionary<string, object> test, Dictionary<string, object> state)
    {
        bool allMatch = true;
        foreach(string key in test.Keys)
        {
            bool match = state.ContainsKey(key) && test[key].Equals(state[key]);
            if (!match)
            {
                allMatch = false;
                break;
            }
                
        }
        return allMatch;
    }

    // Apply the stateChange to the currentState
    private Dictionary<string, object> populateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
    {
        Dictionary<string, object> state = new Dictionary<string, object>(currentState);

        foreach (string key in stateChange.Keys)
        {
            if (state.ContainsKey(key))
            {
                state[key] = stateChange[key];
            } else
            {
                state.Add(key, stateChange[key]);                
            }
        }
        return state;
    }

}


