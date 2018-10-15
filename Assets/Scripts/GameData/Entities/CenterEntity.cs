using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterEntity : MonoBehaviour {

    public int agentsCounts = 0;
    public float bornRate = 100f;
    public float bornLimitRate = 0f;
    public Agent[] agentTypes;
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
                Instantiate(agentTypes[0], new Vector3(transform.position.x, transform.position.y - 0.7f, -2.5f), Quaternion.identity);
            }
        }
    }
}
