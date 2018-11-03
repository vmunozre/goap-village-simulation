using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : Agent {
    // Basic data
    public new string name = "Collector";
    public int food = 0;
    public BushEntity actualBush = null;
    // Use this for initialization
    void Start()
    {
        center.agentsCounter[name]++;
        if(center.needCarriers() && center.agentsCounter[name] >= 2)
        {
            instanciateSuccessor("Carrier");
        }
    }
	
	// Update is called once per frame
	void Update () {
        checkIsAdult();
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
        }

        if (actualBush == null)
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

    private void autoDestroy()
    {
        center.agentsCounter[name]--;
        Destroy(gameObject);
    }

    public void instanciateSuccessor(string _prefab)
    {
        Instantiate(Resources.Load("Prefabs/Agents/" + _prefab), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        autoDestroy();
    }
}
