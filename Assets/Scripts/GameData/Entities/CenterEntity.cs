using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterEntity : MonoBehaviour {

    public int agentsCounts = 0;
    public float bornRate = 100f;
    public float bornLimitRate = 0f;
    public Agent[] agentTypes;
    // Warehouse associated
    public WarehouseEntity warehouse;


    private int numWoodCutters = 1;
    private int numCollector = 1;

    public void enterAgentToRecover()
    {
        agentsCounts++;
        procreate();
    }

    public void exitAgentToRecover()
    {
        agentsCounts--;
    }

    private void procreate()
    {
        if(agentsCounts >= 2)
        {
            float rate = Random.Range(0f, bornRate);
            if(rate > bornLimitRate)
            {
                float randomNum = Random.Range(0f, 100f);

                if (warehouse.wood > warehouse.food)
                {
                    randomNum += 20;
                }
                else
                {
                    randomNum -= 20;
                }
                Debug.Log("[CenterEntity - procreate()]Random num: " + randomNum);
                if (randomNum >= 50)
                {
                    Instantiate(agentTypes[0], new Vector3(transform.position.x, transform.position.y - 0.7f, -2.5f), Quaternion.identity);
                    numCollector++;
                } else
                {
                    Instantiate(agentTypes[1], new Vector3(transform.position.x, transform.position.y - 0.7f, -2.5f), Quaternion.identity);
                    numWoodCutters++;
                }
            }
        }
    }
}
