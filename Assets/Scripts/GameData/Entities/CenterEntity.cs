using System.Collections.Generic;
using UnityEngine;

public class CenterEntity : MonoBehaviour {

    public int actualAgents = 0;
    public int bornCost = 50;

    // Warehouse associated
    public WarehouseEntity warehouse;
    public Dictionary<string, int> agentsCounter;

    public Dictionary<string, Building> buildingsRequests;
    //Tenders
    private Dictionary<string, Hunter> tenders;

    public CenterEntity()
    {
        buildingsRequests = new Dictionary<string, Building>();
        tenders = new Dictionary<string, Hunter>();
        // Add agents counters
        agentsCounter = new Dictionary<string, int>();
        agentsCounter.Add("Builder", 0);
        agentsCounter.Add("Carrier", 0);
        agentsCounter.Add("Collector", 0);
        agentsCounter.Add("Farmer", 0);
        agentsCounter.Add("Fisher", 0);
        agentsCounter.Add("Hunter", 0);
        agentsCounter.Add("Stonecutter", 0);
        agentsCounter.Add("Woodcutter", 0);                
    }

    private void Awake()
    {
        // Add building basic house
        Building building = new Building("Prefabs/Buildings/House", 100, 100, 3, 1);
        addNewBuildingRequest(building);
    }
 
    // Buildings functions
    public void addNewBuildingRequest(Building _building)
    {
        if (!checkBuildingRequests(_building))
        {
            buildingsRequests.Add(_building.prefabPath, _building);            
        }
        
    }
    public void removeBuildingRequest(Building _building)
    {
        if (checkBuildingRequests(_building))
        {

            buildingsRequests.Remove(_building.prefabPath);
        }
    }

    public bool checkBuildingRequests(Building _building)
    {
        return buildingsRequests.ContainsKey(_building.prefabPath);
    }

    public Building getBuildingRequest()
    { 
        Building building = null;
        if (buildingsRequests.Count > 0)
        {
            foreach(string path in buildingsRequests.Keys)
            {
                if (buildingsRequests[path].taken)
                {
                    continue;
                }
                if(building == null)
                {
                    building = buildingsRequests[path];
                } else
                {
                    if(buildingsRequests[path].priority > building.priority)
                    {
                        building = buildingsRequests[path]; 
                    }
                }
            }
            if(building != null)
            {
                building.taken = true;
            }
        } else
        {
            Debug.Log("NO BUILDING REQUESTS");            
        }
        return building;
    }

    // Add tender to hunters
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

    // Enter Agents
    public void enterAgentToRecover()
    {
        actualAgents++;
        procreate();
    }

    public void exitAgentToRecover()
    {
        actualAgents--;
    }

    // Procreation system
    private void procreate()
    {
        if (actualAgents >= 2 && (actualAgents % 2 == 0) && warehouse.food >= bornCost)
        {
            int rate = Random.Range(1, 100);

            if (rate <= 35 && needStoneCutters())
            {
                rate = Random.Range(1, 100);
                if (rate < 25 && needBuilders())
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Builder"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
                else
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Stonecutter"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
            }
            else
            {
                rate = Random.Range(1, 100);

                if (rate < 40 && needWoodcutters())
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Woodcutter"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
                else
                {
                    Instantiate(Resources.Load("Prefabs/Agents/Collector"), new Vector3(transform.position.x, transform.position.y - 0.6f, -3), Quaternion.identity);
                }
            }
            warehouse.food -= bornCost;
        }

    }
    // Need system
    public bool needBuilders()
    {
        bool result = false;

        result = agentsCounter["Builder"] < 2;

        return result;
    }

    public bool needWoodcutters()
    {
        bool result = false;

        result = agentsCounter["Woodcutter"] < 7;

        return result;
    }

    public bool needStoneCutters()
    {
        bool result = false;
        QuarryEntity[] quarries = (QuarryEntity[])FindObjectsOfType(typeof(QuarryEntity));
        foreach (QuarryEntity quarry in quarries)
        {
            if (quarry.full)
            {
                continue;
            }
            result = true;
            break;
        }
        return result;
    }

    public bool needCarriers()
    {
        bool result = false;

        SawmillBuilding[] sawmills = (SawmillBuilding[])FindObjectsOfType(typeof(SawmillBuilding));
        foreach (SawmillBuilding saw in sawmills)
        {
            if (!saw.blueprint.done || saw.carriers >= saw.limitCarriers)
            {
                continue;
            }
            result = true;
            break;
        }
        if (!result)
        {
            HuntingShedBuilding[] huntingSheds = (HuntingShedBuilding[])FindObjectsOfType(typeof(HuntingShedBuilding));
            foreach (HuntingShedBuilding shed in huntingSheds)
            {
                if (!shed.blueprint.done || shed.carriers >= shed.limitCarriers)
                {
                    continue;
                }
                result = true;
                break;
            }
        }

        return result;
    }

    public bool needHunters()
    {
        bool result = false;
        HerdEntity[] herds = (HerdEntity[])FindObjectsOfType(typeof(HerdEntity));
        result = agentsCounter["Hunter"] < (herds.Length + 2);
        
        return result;
    }

    public bool needFishers()
    {
        bool result = false;
        LakeEntity[] lakes = (LakeEntity[])FindObjectsOfType(typeof(LakeEntity));

        result = agentsCounter["Fisher"] < (lakes.Length * 4);        
        return result;
    }
}
