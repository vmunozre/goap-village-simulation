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
    private int numStonecutter = 1;

    //Tenders

    private Dictionary<string, Hunter> tenders;

    public CenterEntity()
    {
        tenders = new Dictionary<string, Hunter>();
    }

    public bool enterAgentToRecover()
    {
        agentsCounts++;
        return procreate();
    }

    public void exitAgentToRecover()
    {
        agentsCounts--;
    }

    public void addTender(string _title, Hunter _object)
    {
        tenders.Add(_title, _object);
    }

    public Hunter checkTender(string _title)
    {
        Hunter tender = null;
        if (tenders.ContainsKey(_title))
        {
            tender = tenders[_title];
            
        }
        return tender;
    }

    private bool procreate()
    {
        if(agentsCounts >= 2 && (agentsCounts % 2 == 0))
        {
            float rate = Random.Range(0f, bornRate);
            if(rate > bornLimitRate)
            {
                float randomNum = Random.Range(0f, 100f);

                if(randomNum < 20)
                {
                    Instantiate(agentTypes[2], new Vector3(transform.position.x, transform.position.y - 0.7f, -2.5f), Quaternion.identity);
                    numStonecutter++;
                } else
                {
                    if (warehouse.wood > warehouse.food)
                    {
                        randomNum += 20;
                    }
                    else
                    {
                        randomNum -= 20;
                    }
                    
                    if (randomNum >= 50)
                    {
                        Instantiate(agentTypes[0], new Vector3(transform.position.x, transform.position.y - 0.7f, -2.5f), Quaternion.identity);
                        numCollector++;
                    }
                    else
                    {
                        Instantiate(agentTypes[1], new Vector3(transform.position.x, transform.position.y - 0.7f, -2.5f), Quaternion.identity);
                        numWoodCutters++;
                    }
                    
                }
                return true;

            }
        }
        return false;
    }
}
