using System.Collections.Generic;
using UnityEngine;

public class Collector : Agent {   
    public int food = 0;
    public BushEntity actualBush = null;

    private new string name = "Collector";

    void Start()
    {
        center.agentsCounter[name]++;

        // Check if village need carriers
        if(center.needCarriers() && center.agentsCounter[name] >= 2)
        {
            instanciateSuccessor("Carrier");
        }
    }
	
	void Update () {
        checkSuperUpdate();
	}

    public override Dictionary<string, object> createGoalState()
    {
        Dictionary<string, object> goal = new Dictionary<string, object>();
        if (waiting)
        {
            goal.Add("waitComplete", true);
            return goal;
        }
        if (actualBush != null)
        {
            goal.Add("collectFood", true);
        } else
        {
            goal.Add("bushFound", true);
        }
        
        return goal;
    }

    public override Dictionary<string, object> getWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();
        worldData.Add("isWaiting", waiting);
        worldData.Add("hasFood", (food > 0));
        worldData.Add("hasEnergy", (energy > 0));
        worldData.Add("hasActualBush", (actualBush != null));

        return worldData;
    }

    // Destroy this collector
    private void autoDestroy()
    {
        center.agentsCounter[name]--;
        Destroy(gameObject);
    }

    // Add successor
    public void instanciateSuccessor(string _prefab)
    {
        Instantiate(Resources.Load("Prefabs/Agents/" + _prefab), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        autoDestroy();
    }
}
