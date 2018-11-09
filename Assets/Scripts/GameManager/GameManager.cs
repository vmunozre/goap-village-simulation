using UnityEngine;

public class GameManager : MonoBehaviour {
    public float actualMuti = 1;

    private CenterEntity center = null;
    private WarehouseEntity warehouse = null;

    public static GameManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
        if (centers.Length > 0)
        {
            center = centers[0];
        }

        WarehouseEntity[] warehouses = (WarehouseEntity[])FindObjectsOfType(typeof(WarehouseEntity));
        if (warehouses.Length > 0)
        {
            warehouse = warehouses[0];
        }

    }
	
	void Update () {
        // Speed controllers
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            actualMuti = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            actualMuti = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            actualMuti = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            actualMuti = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            actualMuti = 10;
        }

        // Force procreate
        if (Input.GetKeyDown(KeyCode.P))
        {
            center.procreateRule();
        }

        // Add resources x100
        if (Input.GetKeyDown(KeyCode.U))
        {
            warehouse.food += 100;
            warehouse.wood += 100;
            warehouse.stone += 100;
        }

    }
}
