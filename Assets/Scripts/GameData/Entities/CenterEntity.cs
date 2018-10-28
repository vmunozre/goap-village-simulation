using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterEntity : MonoBehaviour {

    public int actualAgents = 0;
    public int bornCost = 50;

    // Warehouse associated
    public WarehouseEntity warehouse;
    public Dictionary<string, int> agentsCounter;

    public List<Building> buildingsRequests = new List<Building>();
    //Tenders
    private Dictionary<string, Hunter> tenders;

    public CenterEntity()
    {
        tenders = new Dictionary<string, Hunter>();
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
        Building building = new Building("Prefabs/Buildings/House", 100, 100, 3, 1);
        addNewBuildingRequest(building);
    }

    public void addNewBuildingRequest(Building _building)
    {
        if (!checkBuildingRequests(_building))
        {
            buildingsRequests.Add(_building);
            buildingsRequests.Sort((x, y) => x.priority.CompareTo(y.priority));
        }
        
    }
    public void removeBuildingRequest(Building _building)
    {
        if (checkBuildingRequests(_building))
        {
            // Elimina todos para evitar duplicados
            buildingsRequests.RemoveAll(building => building.prefabPath == _building.prefabPath);
            //buildingsRequests.RemoveAt(index);
        }
    }
    public bool checkBuildingRequests(Building _building)
    {
        Debug.Log("Check Building: " + buildingsRequests.Contains(_building));
        Debug.Log("Check Count: " + buildingsRequests.Count);
        return buildingsRequests.Contains(_building);
    }

    public Building getBuildingRequest()
    { 
        Building building = null;
        if (buildingsRequests.Count > 0)
        {
            building = buildingsRequests[0];
            //buildingsRequests.RemoveAt(0);
        } else
        {
            Debug.Log("NO BUILDING REQUESTS");
            //building = new Building("Prefabs/Buildings/House");
        }
        return building;
    }

    public void enterAgentToRecover()
    {
        actualAgents++;
        procreate();
    }

    public void exitAgentToRecover()
    {
        actualAgents--;
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

    private void procreate()
    {
        if (actualAgents >= 2 && (actualAgents % 2 == 0) && warehouse.food >= bornCost)
        {
            int rate = Random.Range(1, 100);

            if (rate <= 35 && needStoneCutters())
            {
                rate = Random.Range(1, 100);
                if (rate < 15 && needBuilders())
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

    public bool needBuilders()
    {
        bool result = false;

        // 3 por ejemplo
        result = agentsCounter["Builder"] < 1;

        return result;
    }

    public bool needWoodcutters()
    {
        bool result = false;

        // 5 por ejemplo
        result = agentsCounter["Woodcutter"] < 5;

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
