using UnityEngine;

public class Building
{
    public GameObject building;
    // Cost and progress
    public int woodCost = 100;
    public int stoneCost = 100;
    public int actualWood = 0;
    public int actualStone = 0;
    public int priority = 1;
    public int buildEffort = 10;
    public int progress = 0;
    // Taken and done
    public bool done = false;
    public bool taken = false;
    // Prefab like id
    public string prefabPath = "";
    // Not using yet
    public Vector3 position = Vector3.zero;

    public Building(string _prefab, int _woodCost, int _stoneCost, int _buildEffort, int _priority)
    {
        prefabPath = _prefab;
        building = (Resources.Load(_prefab)) as GameObject;
        woodCost = _woodCost;
        stoneCost = _stoneCost;
        buildEffort = _buildEffort;
        priority = _priority;
    }
    // Resources count check
    public bool hasAllResources()
    {
        return actualWood >= woodCost && actualStone >= stoneCost;
    }

}
