using System.Collections.Generic;

public class Node
{
    public Node parent;
    public float runningCost;
    public Dictionary<string, object> state;
    public GoapAction action;

    public Node(Node parent, float runningCost, Dictionary<string, object> state, GoapAction action)
    {
        this.parent = parent;
        this.runningCost = runningCost;
        this.state = new Dictionary<string, object>(state);
        this.action = action;
    }
}