using UnityEngine;
using UnityEngine.UI;
public class ButtonProcreateUI : MonoBehaviour
{
    public Button buttonComponent;
    private CenterEntity center;
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
        CenterEntity[] centers = (CenterEntity[])FindObjectsOfType(typeof(CenterEntity));
        if (centers.Length > 0)
        {
            center = centers[0];
        }
    }

    public void HandleClick()
    {
        center.procreateRule();
    }
}
