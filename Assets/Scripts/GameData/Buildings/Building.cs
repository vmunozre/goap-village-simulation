using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public GameObject building;
    public int woodCost = 100;
    public int stoneCost = 100;
    public int actualWood = 0;
    public int actualStone = 0;
    public int priority = 1;
    public float timeCost = 20f;
    public float progress = 0f;
    public bool done = false;
    public string prefabPath = "";
    public Vector3 position = Vector3.zero;

    public Building(string _prefab, int _woodCost, int _stoneCost, float _timeCost, int _priority)
    {
        prefabPath = _prefab;
        building = (Resources.Load(_prefab)) as GameObject;
        woodCost = _woodCost;
        stoneCost = _stoneCost;
        timeCost = _timeCost;
        priority = _priority;
    }

    public bool hasAllResources()
    {
        return actualWood >= woodCost && actualStone >= stoneCost;
    }

}
