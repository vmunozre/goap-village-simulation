using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : Agent
{
    // Basic data
    public int wood = 0;
    public TreeEntity actualTree = null;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkIsAdult();
    }

    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        if (waiting)
        {
            goal.Add(new KeyValuePair<string, object>("waitComplete", true));
            return goal;
        }

        if (actualTree != null)
        {
            goal.Add(new KeyValuePair<string, object>("collectWood", true));
        }

        if (actualTree == null)
        {
            goal.Add(new KeyValuePair<string, object>("treeFound", true));
        }

        return goal;
    }

    public override HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("isWaiting", waiting));
        worldData.Add(new KeyValuePair<string, object>("hasWood", (wood > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasEnergy", (energy > 0)));
        worldData.Add(new KeyValuePair<string, object>("hasActualTree", (actualTree != null)));
        worldData.Add(new KeyValuePair<string, object>("treeIsChopped", (actualTree != null)&&(actualTree.chopped)));

        return worldData;
    }
}
