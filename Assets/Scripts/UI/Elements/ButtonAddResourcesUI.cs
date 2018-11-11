using UnityEngine;
using UnityEngine.UI;
public class ButtonAddResourcesUI : MonoBehaviour
{
    public Button buttonComponent;
    private WarehouseEntity warehouse;
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
        WarehouseEntity[] warehouses = (WarehouseEntity[])FindObjectsOfType(typeof(WarehouseEntity));
        if (warehouses.Length > 0)
        {
            warehouse = warehouses[0];
        }
    }

    public void HandleClick()
    {
        warehouse.food += 100;
        warehouse.wood += 100;
        warehouse.stone += 100;
    }
}
